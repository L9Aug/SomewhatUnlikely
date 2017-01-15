using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    /// <summary>
    /// Player Controller
    /// </summary>
    public static PlayerController PC;

    public float TakedownFOV;
    public BaseGun CurrentWeapon;
    public bool isADS;

    private List<GameObject> AIInRange = new List<GameObject>(); //list of AI that are in takedown range
    private bool CanTakedown = false;
    private Animator anim;
    private Camera PlayerCam;
    private PauseMenu pauseMenu;
    private PlayerMovementController PMC;
    private EquipmentController equipmentController;
    private CameraController cameraController;

    /// <summary>
    /// Player State Machine
    /// </summary>
    public SM.StateMachine PSM;

    private bool Dying = false;

    public bool Revived = false;

    private float DeathTimer = 0;
    private float DeathLength = 5;

	// Use this for initialization
	void Start ()
    {
        AnimTest();
        PlayerCam = Camera.main;
        pauseMenu = FindObjectOfType<PauseMenu>();
        PMC = GetComponent<PlayerMovementController>();
        equipmentController = GetComponent<EquipmentController>();
        cameraController = GetComponent<CameraController>();
        PC = this;
        SetupStateMachine();
	}
	
	// Update is called once per frame
	void Update ()
    {
        PSM.SMUpdate();
	}

    bool AnimTest()
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }
        return (anim != null) ? true : false;
    }

    public void HealthCheck(float Health, float HealthChanged)
    {
        if(Health <= 0)
        {
            Dying = true;
        }
    }

    void WeaponChecks()
    {
        if (Time.timeScale > 0.01 && PMC.PMSM.GetCurrentState() == "Movement")
        {
            if (CurrentWeapon != null)
            {
                isADS = Input.GetButton("Aim");

                if (Input.GetButton("Fire"))
                {
                    // 1 << 10 is the AI layer.
                    if (CurrentWeapon.Fire(GunTarget, 1 << 10, 0, false, true))
                    {
                        if (AnimTest())
                        {
                            anim.SetTrigger("Fire");
                        }
                    }
                }

                if (Input.GetButton("Reload"))
                {
                    CurrentWeapon.Reload();
                }
            }
            else if(equipmentController.CurrentEquipment == EquipmentController.EquipmentTypes.Distraction)
            {
                if (Input.GetButtonDown("Fire"))
                {

                }
            }

            if (Input.GetButtonDown("CycleEquipment"))
            {
                equipmentController.CycleEquipment();
            }
        }
    }

    Vector3 GunTarget()
    {
        Ray CameraRay = PlayerCam.ScreenPointToRay(new Vector3(0.5f * PlayerCam.pixelWidth, 0.5f * PlayerCam.pixelHeight, 0));
        RaycastHit hit;
        LayerMask mask = 1 << 8; // 1 << 8 is the player layer.
        mask = ~mask; // flip the mask to hit all but the player.
        Debug.DrawRay(CameraRay.origin, CameraRay.direction * 10f, Color.red, 1);
        if(Physics.Raycast(CameraRay, out hit, 1000f, mask))
        {
            return hit.point;
        }
        return (CameraRay.origin + (CameraRay.direction * 1000f));
    }

    void TakedownCheck()
    {
        GameObject TakedownTarget = CheckTakedownFOV();
		CanTakedown = false;

		if (TakedownTarget != null && !Enemy_Patrol.detected) {
			CanTakedown = true;
		}


        if(Input.GetButtonDown("Interact") && CanTakedown)
        {
            PerformTakedown(TakedownTarget);
        }
    }

    void PerformTakedown(GameObject Target)
    {
        if (Target.GetComponent<Base_Enemy>() != null)
        {
            float Takedown = Random.Range(0, 2); // select at random the takedown to use
            Animator TargetAnim = Target.GetComponent<Animator>(); // get the animator of the AI

            Target.GetComponent<Base_Enemy>().setState(Base_Enemy.State.Dead); // turn off the AI

            Target.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Target.GetComponent<NavMeshAgent>().speed = 0;            

            TargetAnim.applyRootMotion = false;
            //Anim.applyRootMotion = true;

            TargetAnim.SetTrigger("Takedown"); // trigger teh animations for both the AI and the Player.
            if (AnimTest())
            {
                anim.SetTrigger("Takedown");
            }

            //Put the player into the takedown state
            PMC.TakedownTarget = Target.transform;
            PMC.BeginTakedown = true;
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
        if(other.GetComponent<Base_Enemy>() != null)
        {
            if (!AIInRange.Contains(other.gameObject))
            {
                AIInRange.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
		if(other.GetComponent<Base_Enemy>() != null)
        {
            if (AIInRange.Contains(other.gameObject))
            {
                AIInRange.Remove(other.gameObject);
            }
        }
    }

    private void ActiveUpdate()
    {
        WeaponChecks();
        TakedownCheck();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (CurrentWeapon != null)
        {
            if (layerIndex == 1 && AnimTest() && PMC.PMSM.GetCurrentState() == "Movement")
            {
                anim.speed = CurrentWeapon.animSpeed;

                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, CurrentWeapon.RightHandPositionWeight);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, CurrentWeapon.LeftHandPositionWeight);
                anim.SetIKPosition(AvatarIKGoal.RightHand, CurrentWeapon.RightHand.position);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, CurrentWeapon.LeftHand.position);

                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, CurrentWeapon.RightHandRotationWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, CurrentWeapon.LeftHandRotationWeight);
                anim.SetIKRotation(AvatarIKGoal.RightHand, CurrentWeapon.RightHand.rotation);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, CurrentWeapon.LeftHand.rotation);
            }
            else if (PMC.PMSM.GetCurrentState() == "Takedown" && AnimTest())
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }

    private void BeginDead()
    {
        DeathTimer = DeathLength;
    }

    private void DeadUpdate()
    {
        DeathTimer -= Time.deltaTime;
        if(DeathTimer <= 0)
        {
            pauseMenu.reloadCheckpoint();
        }
    }

    private void EndDead()
    {
        Dying = false;
    }

    private void DyingTransFunc()
    {
        if (AnimTest())
        {
            anim.SetBool("Dying", true);
            //Dying = false;
        }
    }

    private void RevivedTransFunc()
    {
        if (AnimTest())
        {
            anim.SetBool("Dying", false);
        }
        Revived = false;
    }

    private bool DyingCheck()
    {
        return Dying;
    }

    private bool RevivedCheck()
    {
        return Revived;
    }

    void SetupStateMachine()
    {
        // Configure conditions for transitions.
        Condition.BoolCondition IsDying = new Condition.BoolCondition();
        IsDying.Condition = DyingCheck;

        Condition.BoolCondition IsRevived = new Condition.BoolCondition();
        IsRevived.Condition = RevivedCheck;

        // Create Transitions.
        SM.Transition dying = new SM.Transition("Dying", IsDying, DyingTransFunc);
        SM.Transition revived = new SM.Transition("Revived", IsRevived, RevivedTransFunc);

        // Create States.
        SM.State Active = new SM.State("Active",
            new List<SM.Transition>() { dying },
            new List<SM.Action>() { PMC.BeginFirstPersonState },
            new List<SM.Action>() { PMC.PMSM.SMUpdate, ActiveUpdate },
            new List<SM.Action>() { PMC.EndFirstPersonState });

        SM.State Dead = new SM.State("Dead",
            new List<SM.Transition>() { revived },
            new List<SM.Action>() { BeginDead },
            new List<SM.Action>() { DeadUpdate },
            new List<SM.Action>() { EndDead });

        // Assign target states to transitions.
        dying.SetTargetState(Dead);
        revived.SetTargetState(Active);

        // Create Machine.
        PSM = new SM.StateMachine(null, Active, Dead);

        // Initialize the machine.
        PSM.InitMachine();
    }

}
