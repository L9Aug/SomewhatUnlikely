using UnityEngine;
using System.Collections;
using UnityEditor;


/// <summary>
/// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
/// </summary>


public class Cover : MonoBehaviour
{

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public bool hideReady;
    public float hideTimer = 0;

    public GameObject Player;
    public Vector3 distToTarget;
    private Vector3 closestTarget;
    private Vector3 currentTarget;
    private Vector3 distToPlayer;
    public bool allowCover;
    public float hiddenTimer;
    private float hideLenght = 5;

    NavMeshAgent Agent;
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public void AICover()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask); //creates sphare collider using view radius and the target mask(player) for it to collide with and stores hits as an array
        if (targetsInViewRadius.Length != 0)
        {
            distToPlayer = transform.position - Player.transform.position; // stops ai entering cover if theyre > certian distance from the player
            if (allowCover) // ai won't attempt to search for cover unless they find one in the vicinity
            {
                for (int i = 0; i < targetsInViewRadius.Length; i++) // checks list of targets placed based on whats in the colliding sphare.
                {
                    //determining closest cover point to ai and to send ai there or to continue in chase state based on distance.
                    distToTarget = targetsInViewRadius[i].transform.position - transform.position;
                    if (closestTarget == Vector3.zero)
                    {
                        closestTarget = targetsInViewRadius[i].transform.position - transform.position;
                        currentTarget = targetsInViewRadius[i].transform.position;
                    }
                    else if (closestTarget.magnitude <= 2.2f)
                    {

                        if (!allowCover) // simple timer for ai to remain in cover.
                        {
                            allowCover = true;
                        }
                        else if (hiddenTimer < hideLenght && allowCover)
                        {
                            hiddenTimer += Time.deltaTime;
                        }
                        else if (hiddenTimer >= hideLenght && allowCover)
                        {
                            allowCover = false;
                            hiddenTimer = 0;
                            distToTarget = Vector3.zero;
                            closestTarget = Vector3.zero;
                            GetComponent<AI_Main>().setState(AI_Main.State.Chase);
                        }
                    }
                    else if (closestTarget.magnitude > distToTarget.magnitude)
                    {
                        closestTarget = distToTarget;
                        currentTarget = targetsInViewRadius[i].transform.position;
                    }
                    else if (distToPlayer.magnitude <= 15) // if cover point is close enough to the player ai goes there. (stops ai who are coming from across the map from entering random cover points along the way which is pointless)
                    {
                        Debug.DrawLine(transform.position, targetsInViewRadius[i].transform.position);
                        Agent.speed = 1;
                        Agent.SetDestination(currentTarget);
                    }
                    else
                    {
                        Debug.Log("not in range");
                        //GetComponent<AI_Main>().setState(AI_Main.State.Chase); // if no close cover point then continues to chase until cover is next called.
                    }
                }
            }
            else if (hiddenTimer < hideLenght && !allowCover)
            {
                 hiddenTimer += Time.deltaTime;
            }
            else if (hiddenTimer >= hideLenght && !allowCover)
            {
                hiddenTimer = 0; // reset to zero as currently same variable used when reaching the cover position

                if (distToPlayer.magnitude <= 15) // blocks ai entering cover state in random locations ridiculously far from the player
                {
                    allowCover = true;
                    distToTarget = Vector3.zero; //cleaning values to avoid an annoying bug.
                    closestTarget = Vector3.zero;
                    GetComponent<AI_Main>().setState(AI_Main.State.InCover);
                }
            }
        }
        else
        {
            distToTarget = Vector3.zero; // just resets to zero when not in use, it stops a bug.
            closestTarget = Vector3.zero;
        }
    }

}


#region unityeditor debug visuals
[CustomEditor(typeof(Cover))]
public class CompanionHideEditor : Editor
{
    void OnSceneGUI()
    {
        Cover fow = (Cover)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
    }
}
#endregion