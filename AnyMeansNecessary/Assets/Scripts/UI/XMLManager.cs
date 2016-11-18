using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour {

    public static XMLManager instance;
    public DataBase enemyDB;
    // Use this for initialization
    void Awake () {

        instance = this;
        if (enemyDB.enemList != null)
        {
            
            LoadEnemy();
        }
       
	}

  

 public   void saveEnemy()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DataBase));
        FileStream stream = new FileStream(Application.dataPath + "/DataTest/enemData.xml",FileMode.Create);
        serializer.Serialize(stream, enemyDB);
        stream.Close();

    }

    public void LoadEnemy()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DataBase));
        FileStream stream = new FileStream(Application.dataPath + "/DataTest/enemData.xml", FileMode.Open);
        enemyDB = serializer.Deserialize(stream) as DataBase;
        stream.Close();

    }

    public void xmlstoredata()
    {

        int arrayLength = GameObject.FindGameObjectsWithTag("Enemy").Length;
        for (int i = 0; i < arrayLength; i++)
        {
            if (enemyDB.enemList.Count <= GameObject.FindGameObjectsWithTag("Enemy").Length)
            {
                enemyDB.enemList.Add(new EnemDataToSave(GameObject.FindGameObjectsWithTag("Enemy")[i].transform.position,
                                                        GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<HealthComp>().GetHealth(),
                                                        GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<FieldOfView>().detectedtimer,
                                                        GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<AI_Main>()._state));
            }
            else 
            {
               
            }
        }
        instance.enemyDB.PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthComp>().GetHealth();
        instance.enemyDB.PlayerXP = UIElements.xp;
        instance.enemyDB.PlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;


    }
    }

    [System.Serializable]
public class EnemDataToSave
{
    
    public  Vector3 enemPos;
    public  float enemHealth;
    public  float detectionTimer;
    public  AI_Main.State enemyState;

    public EnemDataToSave(Vector3 pos,float health,float timer, AI_Main.State state)
    {
        enemPos = pos;
        enemHealth = health;
        detectionTimer = timer;
        enemyState = state;
        
    }

    public EnemDataToSave()
    {

    }
}

[System.Serializable]
public class DataBase
{
    
    public List<EnemDataToSave> enemList = new List<EnemDataToSave>();

    public float PlayerHealth;
    public int PlayerXP;
    public int PlayerLevel;
    public int ammoCount;
    public Vector3 PlayerPos;

    public DataBase()
    {

    }
    
}



