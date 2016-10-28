using UnityEngine;
using System.Collections;

public class Distraction : MonoBehaviour {
    public float objectDestroyInSeconds = 10;
    private float objectDestryTimer;
	
	// Update is called once per frame
	void Update () {
        objectDestryTimer += Time.deltaTime;

        if(objectDestryTimer >= objectDestroyInSeconds)
        {
          Destroy(gameObject);
        }
	}
}
