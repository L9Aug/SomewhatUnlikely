using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    public Button Resume;
    public Button Reload;
    public Button Quit;
   


   


    // Use this for initialization
    void Start () {

        disableButtons();
       
	}
	
	// Update is called once per frame
	void Update () {

       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            enableButtons();
            
            Time.timeScale = 0.0f;
            
        }

       


    }


    void disableButtons()// disables pause menu
    {
        Resume.gameObject.SetActive(false);
        Reload.gameObject.SetActive(false);
        Quit.gameObject.SetActive(false);
       
    }

    void enableButtons()//Function brings the pause menu up
    {
        Resume.gameObject.SetActive(true);
        Reload.gameObject.SetActive(true);
        Quit.gameObject.SetActive(true);
       
    }

     void resume() //resumes game
    {
        disableButtons();
        Time.timeScale = 1.0f;
    }

     void quit() //quits game
    {
        Application.Quit();
        Debug.Log("Is Quitting");
    }

    void reloadCheckpoint()//reloads to checkpoint
    {
       GameObject playerPos = GameObject.FindGameObjectWithTag("Player");
        GameObject[] enemyPos = GameObject.FindGameObjectsWithTag("Enemy");

        int arrayLength = enemyPos.Length;
       for(int i = 0; i < arrayLength; i++)
       {
           enemyPos[i].transform.position = CheckpointScript.EnemyPosition[i];//moves all enemies to stored position when the player reached the chekpoint
       }
        playerPos.transform.position = CheckpointScript.GetCheckpointPosition();//moves player to checkpoint position
        UIElements.health = CheckpointScript.storedHealth;// sets health to stored value
        UIElements.xp = CheckpointScript.storedXp;//sets xp to stored value
        

        resume();
        
        
    }

  

  
}
