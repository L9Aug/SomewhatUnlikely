using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthComp : MonoBehaviour {

    public float MaxHealth;
    float health;

    public delegate void HealthChanged(float Health, float ChangeInHealth);
    public List<HealthChanged> healthChanged = new List<HealthChanged>();

	// Use this for initialization
	void Start ()
    {
        health = MaxHealth;
	}

    public float GetHealth()
    {
        return health;
    }
    
    public void SetHealth(float Value)
    {
        float DeltaHealth = Value - health;
        health = Value;
        if (healthChanged.Count > 0)
        {
            foreach (HealthChanged h in healthChanged)
            {
                h(health, DeltaHealth);
            }
        }
    }

    public void Hit(float Amount)
    {
        health -= Amount;
        if(healthChanged.Count > 0)
        {
            foreach(HealthChanged h in healthChanged)
            {
                h(health, Amount);
            }
        }
        //print("Hit");
    }
}
