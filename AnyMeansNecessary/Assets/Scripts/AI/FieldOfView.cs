using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    void Update()
    {
        GetComponent<AI_Main>().checkLost(FindVisibleTargets()); // constantly searches if player is within detection radius
    }

    bool FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask); //creates sphare collider using view radius and the target mask(player) for it to collide with and stores hits as an array
        for (int i = 0; i < targetsInViewRadius.Length; i++) // checks list of targets placed based on whats in the colliding sphare.
        {

            Vector3 dirToTarget = (targetsInViewRadius[i].transform.position - transform.position).normalized; // determines deirection raycast needs to be sent out at.
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) // determines if player is inside the viewing angle and not behind it
            {
                float dstToTarget = Vector3.Distance(transform.position, targetsInViewRadius[i].transform.position); // detemines distance for raycast lenght
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) //raycast setout to check if obstacles(walls etc) are in way of player
                {
                    Debug.DrawLine(transform.position, targetsInViewRadius[i].transform.position, Color.magenta); //simple debug to see that the player has been seen within the scene
                    GetComponent<AI_Main>().setState(AI_Main.State.Chase); //tells ai to chase player
                    return true; // returns true and sets static "detected" from ai.main to true via checklost function to make all ai chase the player
                }
            }
        }
        return false; // returns boolean for ai.main checklost function.
    }

    //for debugging purposes within fieldofvieweditor
    public Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
