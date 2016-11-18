using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    public GameObject PauseButtons;
    public GameObject GamePlayHUD;
    public GameObject Inventory;
    public GameObject Map;

    public GameObject Player;
    public Camera MapCamera;

    
   
    
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

        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryUp();
            Time.timeScale = 0.0f;
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            mapUp();
            Time.timeScale = 0.0f;
        }
    }

    void disableButtons()// disables pause menu
    {
        PauseButtons.gameObject.SetActive(false);
        Inventory.gameObject.SetActive(false);
        Map.gameObject.SetActive(false);
        GamePlayHUD.gameObject.SetActive(true);
        
//#if !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
//#endif
    }

   void enableButtons()//Function brings the pause menu up
    {
        PauseButtons.gameObject.SetActive(true);
        GamePlayHUD.gameObject.SetActive(false);
        Inventory.gameObject.SetActive(false);
        Map.gameObject.SetActive(false);
        //#if !UNITY_EDITOR
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
//#endif
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

    public void inventoryUp()
    {
        Inventory.gameObject.SetActive(true);
        GamePlayHUD.gameObject.SetActive(false);
        PauseButtons.gameObject.SetActive(false);
        Map.gameObject.SetActive(false);

        //#if !UNITY_EDITOR
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //#endif

    }

    public void mapUp()
    {
        //Displays Map section of the menu and deactivates other elements
        Map.gameObject.SetActive(true);
        Inventory.gameObject.SetActive(false);
        GamePlayHUD.gameObject.SetActive(false);
        PauseButtons.gameObject.SetActive(false);



        MapCamera.transform.position = new Vector3(Player.transform.position.x,50,Player.transform.position.z);
        MapCamera.cullingMask |= (1 << 0)|(1<<8)|(1<<9)|(1<<11)|(1<<12);

        //#if !UNITY_EDITOR
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //#endif

    }


    public void reloadCheckpoint()//reloads to checkpoint
    {
        GameObject playerPos = GameObject.FindGameObjectWithTag("Player");
        GameObject[] Enemy = GameObject.FindGameObjectsWithTag("Enemy");

        int listSize = XMLManager.instance.enemyDB.enemList.Capacity;
       for(int i = 0; i < Enemy.Length;i++)
        {
            Enemy[i].transform.position = XMLManager.instance.enemyDB.enemList[i].enemPos;
            Enemy[i].GetComponent<AI_Main>()._state = XMLManager.instance.enemyDB.enemList[i].enemyState;
            Enemy[i].GetComponent<HealthComp>().SetHealth(XMLManager.instance.enemyDB.enemList[i].enemHealth);
            Enemy[i].GetComponent<FieldOfView>().detectedtimer = XMLManager.instance.enemyDB.enemList[i].detectionTimer;

        }
        
       

        playerPos.transform.position = XMLManager.instance.enemyDB.PlayerPos;//moves player to checkpoint position
        playerPos.GetComponent<HealthComp>().SetHealth(XMLManager.instance.enemyDB.PlayerHealth);
        //UIElements.health = CheckpointScript.storedHealth;// sets health to stored value
        UIElements.xp = CheckpointScript.storedXp;//sets xp to stored value        

        resume();
    }

  

}
