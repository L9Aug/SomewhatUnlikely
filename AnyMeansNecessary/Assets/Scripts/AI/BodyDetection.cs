using UnityEngine;
using System.Collections;
using UnityEditor;


/// <summary>
/// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
/// </summary>

public class BodyDetection : MonoBehaviour {

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public float detectedtimer = 0;
    NavMeshAgent Agent;

    void Start()
    {
      Agent = GetComponent<NavMeshAgent>();
    }


    public bool FindBodies()//called in patrol and alerted state
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask); //creates sphare collider using view radius and the target mask(player) for it to collide with and stores hits as an array
        for (int i = 0; i < targetsInViewRadius.Length; i++) // checks list of targets placed based on whats in the colliding sphare.
        {
            if (targetsInViewRadius[i].GetComponent<AI_Main>()._state == AI_Main.State.Dead)
            {
                if (detectedtimer >= FieldOfView.detectionTimer)
                {
                    GetComponent<AI_Main>().setState(AI_Main.State.Alerted);
                    Vector3 distToTarget = transform.position - targetsInViewRadius[i].transform.position;
                    if (distToTarget.magnitude < 5)
                    {
                        Agent.speed = 0;
                        return true;
                    }
                    else
                    {
                        Agent.speed = 0.5f;
                        Agent.SetDestination(targetsInViewRadius[i].transform.position);
                    }
                    Debug.DrawLine(transform.position, targetsInViewRadius[i].transform.position, Color.green); //simple debug to see that the player has been seen within the scene
                    FieldOfView.detectionTimer = 0.5f;
                }
                else
                {
                    detectedtimer += Time.deltaTime;
                    return false;
                }
            }

        }
        if (detectedtimer > 0)
            detectedtimer -= Time.deltaTime;
        return false;
    }



    // for fov editor
    public Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}



#region Debug Editor //allows visualization within the scene viewer
[CustomEditor(typeof(BodyDetection))]
public class BodyDetectionEditor : Editor
{
    void OnSceneGUI()
    {
        ///this is for debugging purposes it allos visualization within the scene window of the ai's detection radius
        BodyDetection fow = (BodyDetection)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
    }
}
#endregion
