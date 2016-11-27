using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

    /// <summary>
    /// Player Movement State Machine
    /// </summary>
    StateMachine PMSM;
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
        SetupStateMachine();
        anim = GetComponent<Animator>();
        uiElements = FindObjectOfType<UIElements>();
	}
	
	// Update is called once per frame
	void Update () {
        PMSM.SMUpdate();
	}

    #region PMSM Functions

    void BeginTakedownState()
    {

    }

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

    void BeginFirstPersonState()
    {
        m_FPM.enabled = true;
    }

    void FirstPersonStateUpdate()
    {

    }

    void EndFirstPersonState()
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
        return (!anim.GetCurrentAnimatorStateInfo(2).IsTag("InTakedown") && anim.GetCurrentAnimatorStateInfo(2).normalizedTime < 0.5f) || (TakedownTarget == null);
    }

    bool TakedownTest()
    {
        return BeginTakedown && (TakedownTarget != null);
    }

    #endregion

    void SetupStateMachine()
    {
        // Create Machine
        PMSM = new StateMachine();

        // Create States
        State TakedownState = new State();
        State FirstPersonState = new State();

        // Create Transistions
        Transition BeginTakedown = new Transition();
        Transition EndTakedown = new Transition();

        // Add States to Machine
        PMSM.States.Add(FirstPersonState);
        PMSM.States.Add(TakedownState);

        // Assign Initial State
        PMSM.InitialState = PMSM.States[0];

        // Assign Actions to States
        TakedownState.EntryActions.Add(BeginTakedownState);
        TakedownState.Actions.Add(TakedownStateUpdate);
        TakedownState.ExitActions.Add(EndTakedownState);
        FirstPersonState.EntryActions.Add(BeginFirstPersonState);
        FirstPersonState.Actions.Add(FirstPersonStateUpdate);
        FirstPersonState.ExitActions.Add(EndFirstPersonState);

        // Assign Transitions to States
        TakedownState.Transitions.Add(EndTakedown);
        FirstPersonState.Transitions.Add(BeginTakedown);

        // Assign Actions to Transitions
        BeginTakedown.Actions.Add(BeginTakedownTransitionFunc);
        EndTakedown.Actions.Add(EndTakedownTransitionFunc);

        // Assign Target States to Transitions
        BeginTakedown.TargetState = TakedownState;
        EndTakedown.TargetState = FirstPersonState;

        // Configure Conditions For Transitions
        BoolCondition BeginTakedownCond = new BoolCondition();
        BeginTakedownCond.Condition = TakedownTest;
        BeginTakedown.condition = BeginTakedownCond;

        BoolCondition EndTakedownCond = new BoolCondition();
        EndTakedownCond.Condition = TestAnimTag;
        EndTakedown.condition = EndTakedownCond;

        // Assign Each State a Name
        TakedownState.StateName = "Takedown Mode";
        FirstPersonState.StateName = "User Control";

        // Assign each Trnsition a Name
        BeginTakedown.TransistionName = "Begin Takedown";
        EndTakedown.TransistionName = "End Takedown";

        // Set if the Machine should print Messages
        PMSM.PrintMessages = false;

        // Start the State Machine
        PMSM.InitMachine();
    }

    #endregion

}
