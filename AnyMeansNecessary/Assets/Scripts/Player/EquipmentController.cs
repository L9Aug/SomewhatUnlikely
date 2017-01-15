using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentController : MonoBehaviour {

    public enum EquipmentTypes { SilencedPistol, SniperRifle, AssaultRifle, Explosives, Tazer, Distraction }

    public List<GameObject> Equipment = new List<GameObject>();

    private int currentWeapon = 0;
    public EquipmentTypes CurrentEquipment;
    private Transform rightHand;
    private bool isAssigningEquipment = false;
    private UIElements UIE;
    private GameObject gunHolder;

    // Use this for initialization
    void Start () {
        UIE = FindObjectOfType<UIElements>();
        SetEquipment(0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetEquipment(int index)
    {
        if (!isAssigningEquipment)
        {
            if(Equipment.Count > 0)
            {
                StartCoroutine(AssignEquipment(Equipment[index]));
            }
        }
    }

    void SetEquipment(EquipmentTypes chosenEquipment)
    {
        if (!isAssigningEquipment)
        {
            GameObject targetEquipment = Equipment.Find(x => x.name == chosenEquipment.ToString());
            if(targetEquipment != null)
            {
                CurrentEquipment = chosenEquipment;
                StartCoroutine(AssignEquipment(targetEquipment));
            }
        }
    }

    public void CycleEquipment()
    {
        if (!isAssigningEquipment)
        {
            StartCoroutine(AssignEquipment(Equipment[(++currentWeapon) % Equipment.Count]));
            for(int i = 0; i < 5; ++i)
            {
                if (Equipment[currentWeapon % Equipment.Count].name == ((EquipmentTypes)i).ToString())
                {
                    CurrentEquipment = (EquipmentTypes)i;
                    break;
                }
            }
        }
    }

    bool GetGunHolder()
    {
        if(rightHand != null)
        {
            if (gunHolder == null)
            {
                Transform[] handChildren = rightHand.GetComponentsInChildren<Transform>();
                foreach (Transform t in handChildren)
                {
                    if (t.gameObject.name == "GunHolder")
                    {
                        gunHolder = t.gameObject;
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator AssignEquipment(GameObject equipment)
    {
        isAssigningEquipment = true;

        if (equipment.GetComponent<BaseGun>() != null)
        {
            do
            {
                if (GetRightHand())
                {
                    if (!GetGunHolder())
                    {
                        gunHolder = new GameObject();
                        gunHolder.transform.position = Vector3.zero;
                        gunHolder.transform.rotation = Quaternion.identity;
                        gunHolder.transform.SetParent(rightHand, false);
                        gunHolder.name = "GunHolder";
                    }
                    else
                    {
                        if (gunHolder.transform.childCount >0)
                        {
                            Destroy(gunHolder.transform.GetChild(0).gameObject);
                        }
                    }

                    GameObject nEquipment = (GameObject)Instantiate(equipment, Vector3.zero, Quaternion.identity);

                    nEquipment.transform.position = new Vector3(-0.0901f, -0.0428f, 0.03421f);
                    nEquipment.transform.rotation = Quaternion.Euler(184.443f, 91.212f, -15.281f);
                    nEquipment.transform.SetParent(gunHolder.transform, false);

                    GetComponent<PlayerController>().CurrentWeapon = nEquipment.GetComponent<BaseGun>();

                    if (UIE != null)
                    {
                        nEquipment.GetComponent<BaseGun>().updateWeapon = UIE.UpdateWeaponStats;
                    }

                }
                else
                {
                    yield return null;
                }


            } while (rightHand == null);
        }
        else
        {
            Destroy(PlayerController.PC.CurrentWeapon.gameObject);
            PlayerController.PC.CurrentWeapon = null;
        }
        isAssigningEquipment = false;
    }

    bool GetRightHand()
    {
        // if the right hand transform is null then look for the right hand.
        if(rightHand == null)
        {
            Transform[] childTransforms = GetComponentsInChildren<Transform>();

            foreach(Transform trans in childTransforms)
            {
                // once we have found the right hand set the right hand variable and return true to say that it exists.
                if(trans.name == "hand_R")
                {
                    rightHand = trans;
                    return true;
                }
            }
            // if we don't find the right hand then return false to show that it couldn't be found.
            return false;
        }
        return true;        
    }

}
