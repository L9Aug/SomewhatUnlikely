using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UMA {

    public class AddPistolSlotScript : MonoBehaviour {

        public GameObject Pistol;

        public void OnDnaApplied(UMAData umaData)
        {
            List<Transform> children = new List<Transform>();
            children.AddRange(umaData.GetComponentsInChildren<Transform>());
            Transform RightHand = null;

            foreach (Transform t in children)
            {
                if (t.name == "hand_R")
                {
                    RightHand = t;
                    break;
                }
            }

            GameObject GunHolder = new GameObject();
            GunHolder.transform.position = new Vector3(0.0864f, 0.045f, -0.01f);
            GunHolder.transform.rotation = Quaternion.Euler(0.128f, -2.693f, 2.593f);
            GunHolder.transform.SetParent(RightHand, false);

            GameObject pistol = (GameObject)Instantiate(Pistol, Vector3.zero, Quaternion.identity);

            pistol.transform.position = new Vector3(-0.1924f, 0, 0);
            pistol.transform.rotation = Quaternion.Euler(0, -90f, 0);
            pistol.transform.SetParent(GunHolder.transform, false);

            umaData.gameObject.GetComponent<PlayerController>().CurrentWeapon = pistol.GetComponent<Gun>();
            pistol.GetComponent<Gun>().updateWeapon = FindObjectOfType<UIElements>().UpdateWeaponStats;
            //print(umaData.gameObject.GetComponent<PlayerController>());

        }
    }
}
