using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DetectionMeter : MonoBehaviour {

    public Image meter;
    public Camera PlayerCam;
    
   

  public FieldOfView EnemyFOVScript;
    
  float timer;
	// Use this for initialization
	void Start () {
       
        meter.type = Image.Type.Filled;
       
        
        

       
	}
	
	// Update is called once per frame
	void Update () {

        
        transform.LookAt(transform.position + PlayerCam.transform.rotation * Vector3.forward, PlayerCam.transform.rotation * Vector3.up); // billboards detection meter to always be facing the camera
        DetetionTimer();

    }

    void DetetionTimer()
    {

        timer = EnemyFOVScript.detectedtimer;
        meter.fillAmount = timer/FieldOfView.detectionTimer ;


    }
}
