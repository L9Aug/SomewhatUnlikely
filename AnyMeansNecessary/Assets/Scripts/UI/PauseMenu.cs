﻿using UnityEngine;
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

        if (Input.GetButtonDown("Pause"))
        {
            if (Time.timeScale < 0.1f)
            {
                resume();
            }
            else
            {
                enableButtons();
                Time.timeScale = 0.0f;
            }
        }

        if(Input.GetButtonDown("Inventory"))
        {
            inventoryUp();
            Time.timeScale = 0.0f;
        }

        if(Input.GetButtonDown("Map"))
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
        ReloadAI();

        ReloadPlayer();

        resume();
    }

    void ReloadAI()
    {
        Base_Enemy[] Enemy = FindObjectsOfType<Base_Enemy>();

        for (int i = 0; i < Enemy.Length; i++)
        {
            if (Enemy[i] != null)
            {
                if (Enemy[i]._state == Base_Enemy.State.Dead)
                {
                    Enemy[i].GetComponent<Animator>().SetTrigger("Revived");
                }

                Enemy[i].transform.position = XMLManager.instance.enemyDB.enemList[i].enemPos;
                Enemy[i].transform.rotation = XMLManager.instance.enemyDB.enemList[i].enemyRot;
                Enemy[i].GetComponent<Base_Enemy>()._state = Base_Enemy.State.Patrol;
                Enemy[i].GetComponent<Base_Enemy>()._state = XMLManager.instance.enemyDB.enemList[i].enemyState;
                Enemy[i].GetComponent<HealthComp>().SetHealth(XMLManager.instance.enemyDB.enemList[i].enemHealth);
                Enemy[i].GetComponent<FieldOfView>().detectedtimer = XMLManager.instance.enemyDB.enemList[i].detectionTimer;
                Enemy_Patrol.detected = XMLManager.instance.enemyDB.enemList[i].detected;
                Enemy[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    void ReloadPlayer()
    {

        PlayerController.PC.transform.rotation = XMLManager.instance.enemyDB.PlayerRot;
        PlayerController.PC.GetComponent<HealthComp>().SetHealth(XMLManager.instance.enemyDB.PlayerHealth);
        if (XMLManager.instance.enemyDB.PlayerHealth > 0)
        {
            PlayerController.PC.Revived = true;
        }
        //UIElements.health = CheckpointScript.storedHealth;// sets health to stored value
        UIElements.xp = CheckpointScript.storedXp;//sets xp to stored value 
        PlayerController.PC.transform.position = XMLManager.instance.enemyDB.PlayerPos;//moves player to checkpoint position
    }

}
