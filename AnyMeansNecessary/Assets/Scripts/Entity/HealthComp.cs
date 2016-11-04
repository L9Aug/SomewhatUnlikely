using UnityEngine;
using System.Collections;

public class HealthComp : MonoBehaviour {

    public float MaxHealth;
    float Health;

    public delegate void HealthChanged();
    public HealthChanged healthChanged = null;

	// Use this for initialization
	void Start () {
        Health = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Hit(float Amount)
    {
        Health -= Amount;
        if(healthChanged != null) healthChanged();
        print("Hit");
    }
}
