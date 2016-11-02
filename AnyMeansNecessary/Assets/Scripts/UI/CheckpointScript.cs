using UnityEngine;
using System.Collections;

public class CheckpointScript : MonoBehaviour {

    public bool isActivated;
    public static GameObject[] checkPointList;
    public static GameObject[] EnemyList;

    // Use this for initialization
    void Start () {
        checkPointList = GameObject.FindGameObjectsWithTag("CheckPoint"); //Finds all the checkpoints in the level by searching for the tag	
        EnemyList = GameObject.FindGameObjectsWithTag("Enemy"); //Finds all the enemies in the scene, tag all enmeies with this tag so that there position can also be reset to the poin of checkpoint being activated
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    private void ActivateCheckpoint()
    {
        foreach (GameObject checkpoint in checkPointList)
        {
            checkpoint.GetComponent<CheckpointScript>().isActivated = false;
           
        }
        isActivated = true;
        
    }

    public static Vector3 GetCurrentPositions()
    {
        Vector3 posValue = new Vector3(0.0f, 0.418f, 0.0f);
        if (checkPointList != null)
        {
            foreach (GameObject checkpoint in checkPointList)
            {
               if(checkpoint.GetComponent<CheckpointScript>().isActivated == true)
                {
                    posValue = checkpoint.transform.position;
                    break;
                }
            }
        }

        return posValue;
    }

    void OnTriggerEnter(Collider checkpointCollider)
    {
        if(checkpointCollider.tag == "Player")
        {
            ActivateCheckpoint();
        }
    }
}
