using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
/// </summary>


public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public static float detectionTimer = 1.5f;
    public float detectedtimer = 0;
    NavMeshAgent Agent;
    public GameObject Player;
    private Vector3 distToPlayer;

    private Animator Anim;
    public Gun CurrentWeapon;
    void Start()
    {
        Anim = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
    }

   public bool FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask); //creates sphare collider using view radius and the target mask(player) for it to collide with and stores hits as an array
        for (int i = 0; i < targetsInViewRadius.Length; i++) // checks list of targets placed based on whats in the colliding sphare.
        {
            Vector3 dirToTarget = (targetsInViewRadius[i].transform.position - transform.position).normalized; // determines direction raycast needs to be sent out at.
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) // determines if player/body is inside the viewing angle
            {
                float dstToTarget = Vector3.Distance(transform.position, targetsInViewRadius[i].transform.position); // detemines distance for raycast lenght
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) //raycast setout to check if obstacles(walls etc) are in way of player
                {
                    if(targetsInViewRadius[i].gameObject.tag == "Player")
                    {
                        Debug.DrawLine(transform.position, targetsInViewRadius[i].transform.position, Color.green); //simple debug to see that the player has been seen within the scene
                        if (detectedtimer >= detectionTimer)
                        {
                            distToPlayer = transform.position - Player.transform.position;
                            if (distToPlayer.magnitude < 10)
                            { 
                                if (CurrentWeapon.Fire(targetsInViewRadius[i].transform.position, 1 << 8, 0, true))
                                {
                                    Anim.SetTrigger("Fire");
                                }
                             }
                            GetComponent<AI_Main>().setState(AI_Main.State.Chase); //tells ai to chase player
                            return true; // returns true and sets static "detected" from ai.main to true via checklost function to make all ai chase the player
                        }
                        else
                        {
                            detectedtimer += Time.deltaTime;
                            return false;
                        }
                    }
                }
            }
        }
        if (detectedtimer > 0)
            detectedtimer -= Time.deltaTime;
        return false; // returns boolean for ai.main checklost function.
    }


    // for fov editor
    public Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


}

#if UNITY_EDITOR
#region FieldOfView Editor //allows visualization within the scene viewer
[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{

    void OnSceneGUI()
    {
        ///this is for debugging purposes it allos visualization within the scene window of the ai's detection radius
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

    }
}
#endregion
#endif