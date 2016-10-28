using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

    /// <summary>
    /// Player Movement State Machine
    /// </summary>
    StateMachine PMSM;
    bool CamModeChange = false;

    public FirstPersonMovement m_FPM;
    public ThirdPersonMovement m_TPM;

    public static bool PlayerCrouching = false;

	// Use this for initialization
	void Start () {
        SetupStateMachine();
	}
	
	// Update is called once per frame
	void Update () {
        TestForCamModeChange();

        PMSM.SMUpdate();
	}

    void TestForCamModeChange()
    {
        if (Input.GetButtonDown("CamSwitch"))
        {
            CamModeChange = true;
        }
    }

    #region PMSM Functions

    void BeginThirdPersonState()
    {
        m_FPM.enabled = false;
        m_TPM.enabled = true;
    }

    void ThirdPersonStateUpdate()
    {

    }

    void EndThirdPersonState()
    {
        m_TPM.enabled = false;
    }

    void BeginFirstPersonState()
    {
        m_TPM.enabled = false;
        m_FPM.enabled = true;
    }

    void FirstPersonStateUpdate()
    {

    }

    void EndFirstPersonState()
    {
        m_FPM.enabled = false;
    }

    void GoToFirstPersonTransitionFunc()
    {

    }

    void GoToThirdPersonTransitionFunc()
    {

    }

    void ResetCamSwitchBool()
    {
        CamModeChange = false;
    }

    #region PMSM Condition Functions

    bool ChangeCamMode()
    {
        return CamModeChange;
    }

    #endregion

    void SetupStateMachine()
    {
        // Create Machine
        PMSM = new StateMachine();

        // Create States
        State ThirdPersonState = new State();
        State FirstPersonState = new State();

        // Create Transistions
        Transition GoToFirst = new Transition();
        Transition GoToThird = new Transition();

        // Add States to Machine
        PMSM.States.Add(ThirdPersonState);
        PMSM.States.Add(FirstPersonState);

        // Assign Initial State
        PMSM.InitialState = PMSM.States[0];

        // Assign Actions to States
        ThirdPersonState.EntryActions.Add(BeginThirdPersonState);
        ThirdPersonState.Actions.Add(ThirdPersonStateUpdate);
        ThirdPersonState.ExitActions.Add(EndThirdPersonState);
        FirstPersonState.EntryActions.Add(BeginFirstPersonState);
        FirstPersonState.Actions.Add(FirstPersonStateUpdate);
        FirstPersonState.ExitActions.Add(EndFirstPersonState);

        // Assign Transitions to States
        ThirdPersonState.Transitions.Add(GoToFirst);
        FirstPersonState.Transitions.Add(GoToThird);

        // Assign Actions to Transitions
        GoToFirst.Actions.Add(GoToFirstPersonTransitionFunc);
        GoToThird.Actions.Add(GoToThirdPersonTransitionFunc);
        GoToFirst.Actions.Add(ResetCamSwitchBool);
        GoToThird.Actions.Add(ResetCamSwitchBool);

        // Assign Target States to Transitions
        GoToFirst.TargetState = FirstPersonState;
        GoToThird.TargetState = ThirdPersonState;

        // Configure Conditions For Transitions
        BoolCondition CamSwitchCond = new BoolCondition();
        CamSwitchCond.Condition = ChangeCamMode;
        GoToFirst.condition = GoToThird.condition = CamSwitchCond;

        // Assign Each State a Name
        ThirdPersonState.StateName = "Third Person Mode";
        FirstPersonState.StateName = "First Person Mode";

        // Assign each Trnsition a Name
        GoToFirst.TransistionName = "Going To First Person Mode";
        GoToThird.TransistionName = "Going To Third Person Mode";

        // Set if the Machine should print Messages
        PMSM.PrintMessages = false;

        // Start the State Machine
        PMSM.InitMachine();
    }

    #endregion

}
