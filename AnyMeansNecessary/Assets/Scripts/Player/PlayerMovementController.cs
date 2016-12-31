using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class PlayerMovementController : MonoBehaviour {

    /// <summary>
    /// Player Movement State Machine
    /// </summary>
    public SM.StateMachine PMSM;
    public bool BeginTakedown = false;
    public FirstPersonMovement m_FPM;
    private Animator anim;

    public static bool PlayerCrouching = false;

    public Transform TakedownTarget;

    private Vector3 TargetPosition;
    private Quaternion TargetRotation;

    private UIElements uiElements;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        uiElements = FindObjectOfType<UIElements>();
        SetupStateMachine();
	}
	
	// Update is called once per frame
	void Update () {
        //PMSM.SMUpdate();
	}

    bool AnimTest()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        return (anim != null) ? true : false;
    }

    #region PMSM Functions

    void TakedownStateUpdate()
    {
        if(TakedownTarget != null)
        {
            TargetPosition = TakedownTarget.position + (Quaternion.FromToRotation(Vector3.forward, TakedownTarget.forward) * new Vector3(-0.14f, 0, -1.183f));
            TargetRotation = TakedownTarget.rotation;

            UpdatePlayerTransform();
        }
    }

    void UpdatePlayerTransform()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, Time.deltaTime);
    }

    void EndTakedownState()
    {
        uiElements.xpGain(25);
    }

    public void BeginFirstPersonState()
    {
        m_FPM.enabled = true;
    }

    public void EndFirstPersonState()
    {
        m_FPM.enabled = false;
    }

    void BeginTakedownTransitionFunc()
    {
        BeginTakedown = false;        
    }

    void EndTakedownTransitionFunc()
    {
        BeginTakedown = false;
        TakedownTarget = null;
    }

    #region PMSM Condition Functions

    bool TestAnimTag()
    {
        if (AnimTest())
        {
            return (!anim.GetCurrentAnimatorStateInfo(2).IsTag("InTakedown") && anim.GetCurrentAnimatorStateInfo(2).normalizedTime < 0.5f) || (TakedownTarget == null);
        }
        else
        {
            return false;
        }
    }

    bool TakedownTest()
    {
        return BeginTakedown && (TakedownTarget != null);
    }

    #endregion

    void SetupStateMachine()
    {
        // Configure Conditions For Transitions
        Condition.BoolCondition BeginTakedownCond = new Condition.BoolCondition();
        BeginTakedownCond.Condition = TakedownTest;

        Condition.BoolCondition EndTakedownCond = new Condition.BoolCondition();
        EndTakedownCond.Condition = TestAnimTag;

        // Create Transistions
        SM.Transition BeginTakedown = new SM.Transition("Begin Takedown", BeginTakedownCond, BeginTakedownTransitionFunc);
        SM.Transition EndTakedown = new SM.Transition("End Takedown", EndTakedownCond, EndTakedownTransitionFunc);

        // Create States
        SM.State TakedownState = new SM.State("Takedown",
            new List<SM.Transition>() { EndTakedown },
            null,
            new List<SM.Action>() { TakedownStateUpdate },
            new List<SM.Action>() { EndTakedownState });

        SM.State FirstPersonState = new SM.State("Movement",
            new List<SM.Transition>() { BeginTakedown },
            new List<SM.Action>() { BeginFirstPersonState },
            null,
            new List<SM.Action>() { EndFirstPersonState });

        // Assign Target States to Transitions
        BeginTakedown.SetTargetState(TakedownState);
        EndTakedown.SetTargetState(FirstPersonState);

        // Create Machine
        PMSM = new SM.StateMachine(null, FirstPersonState, TakedownState);

        // Start the State Machine
        PMSM.InitMachine();
    }

    #endregion

}
