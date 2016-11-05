using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public float TakedownFOV;
    public Gun CurrentWeapon;

    private List<GameObject> AIInRange = new List<GameObject>(); //list of AI that are in takedown range
    private bool CanTakedown = false;
    private Animator Anim;
    private Camera PlayerCam;

	// Use this for initialization
	void Start ()
    {
        Anim = GetComponent<Animator>();
        PlayerCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
    {
        WeaponChecks();
        TakedownCheck();
	}

    void WeaponChecks()
    {
        if (Input.GetButton("Fire"))
        {
            // 1 << 10 is the AI layer.
            if (CurrentWeapon.Fire(GunTarget, 1 << 10, 0, true))
            {
                Anim.SetTrigger("Fire");
            }
        }

        if (Input.GetButton("Reload"))
        {
            CurrentWeapon.Reload();
        }
    }

    Vector3 GunTarget()
    {
        Ray CameraRay = PlayerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        LayerMask mask = 1 << 8; // 1 << 8 is the player layer.
        mask = ~mask; // flip the mask to hit all but the player.
        if(Physics.Raycast(CameraRay, out hit, 1000f, mask))
        {
            return hit.point;
        }
        return (CameraRay.origin + (CameraRay.direction * 1000f));
    }

    void TakedownCheck()
    {
        GameObject TakedownTarget = CheckTakedownFOV();

        if(TakedownTarget != null && !AI_Main.detected)
        {
            CanTakedown = true;
        }
        else
        {
            CanTakedown = false;
        }

        if(Input.GetButtonDown("Interact") && CanTakedown)
        {
            PerformTakedown(TakedownTarget);
        }
    }

    void PerformTakedown(GameObject Target)
    {
        float Takedown = Random.Range(0, 2); // select at random the takedown to use
        Animator TargetAnim = Target.GetComponent<Animator>(); // get the animator of the AI

        Target.GetComponent<AI_Main>().setState(AI_Main.State.Dead); // turn off the AI

        TargetAnim.applyRootMotion = false;
        //Anim.applyRootMotion = true;

        TargetAnim.SetFloat("Takedowns", Takedown); // tell the animator which takedown to use.
        Anim.SetFloat("Takedowns", Takedown);

        TargetAnim.SetTrigger("Takedown"); // trigger teh animations for both the AI and the Player.
        Anim.SetTrigger("Takedown");
    }

    void AnimTest()
    {
        if(Anim == null)
        {
            Anim = GetComponent<Animator>();
        }
    }

    GameObject CheckTakedownFOV()
    {
        if(AIInRange.Count > 0)
        {
            GameObject returnObject = null;

            List<GameObject> AIInFoV = new List<GameObject>();

            for(int i = 0; i < AIInRange.Count; ++i) //search for the ai that is most in front of the player.
            {
                Vector3 DirToTarget = AIInRange[i].transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, DirToTarget);
                if(angle < (TakedownFOV / 2f))
                {
                    AIInFoV.Add(AIInRange[i]);
                }
            }

            float SmallestDist = 3;

            for (int i = 0; i < AIInFoV.Count; ++i) // of the AI that are in the FoV of the player find the one closest to the player.
            {
                float testDist = Vector3.Distance(transform.position, AIInFoV[i].transform.position);
                if(testDist < SmallestDist)
                {
                    SmallestDist = testDist;
                    returnObject = AIInFoV[i];
                }
            }

            return returnObject;
        }
        
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "AI")
        {
            if (!AIInRange.Contains(other.gameObject))
            {
                AIInRange.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "AI")
        {
            if (AIInRange.Contains(other.gameObject))
            {
                AIInRange.Remove(other.gameObject);
            }
        }
    }
}
