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

    void reloadCheckpoint()
    {
       GameObject playerPos = GameObject.FindGameObjectWithTag("Player");

        playerPos.transform.position = CheckpointScript.GetCurrentPositions();

        resume();
        
        
    }

   
}
