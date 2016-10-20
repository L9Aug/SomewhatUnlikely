using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIElements : MonoBehaviour {


    public int health;
    public int ammo;
    public bool healthloss;
    public bool swapWeapon;
    public Slider healthBar;
    public Toggle mainObjective;
    public Text ammoCount;
    public Text reload;

    public Texture rifle;
    public Texture tranqPistol;
    public RawImage weaponImage;

    // Use this for initialization
    void Start () {

        ammoCount.text = ammo.ToString() + "/30";
	}
	
	// Update is called once per frame
	void Update () {

        //if(healthUsed == true)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            healthloss = false;
            if (health < 100)
            {
                changeHealth(10,healthloss);
            }
        }

        //if(hit == true)
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            healthloss = true;
            if (health > 0)
            {
                changeHealth(10,healthloss);
            }
        }

        // if(objComplete == true) 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            toggleObjective(true);
        }

        if(Input.GetKeyDown(KeyCode.Y))
        {
            swapWeapon = true;
            toggleweaponSprite(swapWeapon);
        }

        if (Input.GetKeyDown(KeyCode.Y) && swapWeapon == true)
        {
            swapWeapon = false;
            toggleweaponSprite(swapWeapon);
        }

        if(Input.GetMouseButtonDown(0))
        {
            firing();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ammo = 30;
            ammoCount.text = ammo.ToString() + "/30";
            reload.text = "";
        }

    }


    public void firing ()
    {
        if (ammo > 0)
        {
            ammo--;
            ammoCount.text = ammo.ToString() + "/30";
        }

        if(ammo == 0)
        {
            reload.text = "Press R to Reload!";
        }
    }



    public void changeHealth(int value,bool change)
    {
        if (change == false)
        {
            health += value;
        }

        if(change == true)
        {
            health -= value;
        }
        healthBar.value = health;
    }

    public void toggleObjective(bool isComplete)
    {
        mainObjective.isOn = isComplete;
    }

    public void toggleweaponSprite(bool isSwapped)
    {
        if(isSwapped == true)
        {
            weaponImage.texture = (Texture)tranqPistol;
        }

        if(isSwapped == false)
        {
            weaponImage.texture = (Texture)rifle;
        }
    }
}
