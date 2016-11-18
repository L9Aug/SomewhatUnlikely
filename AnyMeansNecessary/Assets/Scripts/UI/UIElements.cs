using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIElements : MonoBehaviour {

    public static float health = 100;
    static int ammo = 30;
    public static int xp = 0;
    private float level = 1;
    private bool healthloss;
    private bool swapWeapon;

    public Slider healthBar;
    public Slider xpBar;
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
        //healthBar.value = health;
        float requiredXpForLevel = 25 * (Mathf.Pow(level, 2) + level + 2);
        xpBar.value = (xp / requiredXpForLevel) * 100;

#region keyintputs 
        /*
        //if(healthUsed == true)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            healthloss = false;
            if (health < 100)
            {
                changeHealth(10,healthloss);
            }
            xpGain(10,requiredXpForLevel);
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
        */

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

        /*if(Input.GetMouseButtonDown(0))
        {
            firing();
        }*/

        /*
        if(Input.GetKeyDown(KeyCode.R))
        {
            ammo = 30;
            ammoCount.text = ammo.ToString() + "/30";
            reload.text = "";
        }
        */
#endregion
    }

    #region TristanFuncs
    public void UpdateWeaponStats(int Ammo, int MaxAmmo)
    {
        //ammo = Ammo;
        ammoCount.text = Ammo + " / " + MaxAmmo;
        if(Ammo <= 0)
        {
            reload.text = "Press 'R' to Reload!";
        }
        else
        {
            reload.text = "";
        }
    }

    public void UpdateHealth(float Health, float ChangeInHealth)
    {
        healthBar.value = Health;
    }
    #endregion

    private void firing ()
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

    private void changeHealth(int value,bool change)
    {
        if (change == false)
        {
            health += value;
        }
        else
        {
            health -= value;
        }               
    }

    private void toggleObjective(bool isComplete)
    {
        mainObjective.isOn = isComplete;
    }

    private void toggleweaponSprite(bool isSwapped)
    {
        if(isSwapped == true)
        {
            weaponImage.texture = (Texture)tranqPistol;
        }
        else
        {
            weaponImage.texture = (Texture)rifle;
        }
    }

    private void xpGain(int gain,float requiredXP) // call this function with the amount of xp you wish to add for the player and the requiredXpforLevel float
    {
        //next level equation is 25n^2 + 25n + 50
        xp += gain;
       
        if (xp == requiredXP)
        {
            level++;
            xp = 0;  //if we want pool to reset for each level
        }
    }

   
}
