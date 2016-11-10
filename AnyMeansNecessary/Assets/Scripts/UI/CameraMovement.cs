using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public Camera mapCamera;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    if(Time.timeScale == 0.0f) //if time is paused camera controls active
        {
            
            #region INPUTS
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * 10 * Time.unscaledDeltaTime); //using up as camera is rotated 
               transform.position = new Vector3( Mathf.Clamp(transform.position.x, -150f, 150f),transform.position.y, Mathf.Clamp(transform.position.z, -150f, 150f));
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * 10 * Time.unscaledDeltaTime);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -150f, 150f), transform.position.y, Mathf.Clamp(transform.position.z, -150f, 150f));
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * 10 * Time.unscaledDeltaTime);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -150f, 150f), transform.position.y, Mathf.Clamp(transform.position.z, -150f, 150f));
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * 10 * Time.unscaledDeltaTime);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -150f, 150f), transform.position.y, Mathf.Clamp(transform.position.z, -150f, 150f));
            }
                #endregion
        }
        else 
        {
            mapCamera.cullingMask &= ~(1 << 0) & ~(1 << 8) & ~(1 << 9) & ~(1 << 11) & ~(1 << 12); //removes all rendering from camera when unpaused;
        }
            
                     }
	}

