using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

    StateMachine CameraStates;

    Camera m_Camera;

    public Vector3 FirstPersonPosition;
    public float ThirdPersonCameraDistance;
    public Vector3 ThirdPersonAnchor;

    Transform CameraRootObject;

    bool CamModeChange = false;

    float HorizontalAngle = 0;
    float VerticalAngle = -(Mathf.PI / 2);

    bool InvertMouse = false;

    float LerpTimer = 1.1f;
    float CameraRestTimer = 2;
    float CameraRestDuration = 0.5f;

    float HorizontalRestPoint = 0;
    float VerticalRestPoint = 0;

	// Use this for initialization
	void Start ()
    {
        m_Camera = Camera.main;

        GameObject CameraRootObj = new GameObject("Camera Root Obj");
        CameraRootObject = CameraRootObj.transform;
        CameraRootObj.transform.SetParent(transform, false);
        CameraRootObject.localPosition = ThirdPersonAnchor;
        m_Camera.transform.SetParent(CameraRootObject, false);
        UpdateCameraPosition();

        SetupStateMachine();
	}
	
	// Update is called once per frame
	void Update ()
    {
        TestForCamModeChange();

        List<Action> actions = CameraStates.SMUpdate();
        foreach(Action a in actions)
        {
            a();
        }
	}

    void TestForCamModeChange()
    {
        if(Input.GetButtonDown("CamSwitch"))
        {
            CamModeChange = true;
        }
    }

    void OnCameraMove()
    {
        //reset timer and update start point.
        CameraRestTimer = 0;
        LerpTimer = 0;

        HorizontalRestPoint = HorizontalAngle;
        VerticalRestPoint = VerticalAngle;
    }

    void UpdateCameraPosition()
    {
        m_Camera.transform.localPosition = new Vector3(
            ThirdPersonCameraDistance * Mathf.Sin(VerticalAngle) * Mathf.Sin(HorizontalAngle),
            ThirdPersonCameraDistance * Mathf.Cos(VerticalAngle),
            ThirdPersonCameraDistance * Mathf.Sin(VerticalAngle) * Mathf.Cos(HorizontalAngle));
        m_Camera.transform.LookAt(CameraRootObject);
    }

    void ResetCameraDisplacement()
    {
        if(CameraRestTimer > CameraRestDuration)
        {
            if(LerpTimer < 1)
            {
                HorizontalAngle = Mathf.Lerp(HorizontalRestPoint * Mathf.Rad2Deg, 0, LerpTimer) * Mathf.Deg2Rad;
                VerticalAngle = Mathf.Lerp(VerticalRestPoint * Mathf.Rad2Deg, -90, LerpTimer) * Mathf.Deg2Rad;
                LerpTimer += Time.deltaTime;
            }
            else
            {
                LerpTimer = 1;
            }
        }
        else
        {
            CameraRestTimer += Time.deltaTime;
        }
    }

    #region CameraStateFuctions

    void BeginThirdPersonState()
    {

    }

    void ThirdPersonStateUpdate()
    {
        float HorizontalDelta = Input.GetAxis("Mouse X") * Time.deltaTime;
        float VerticalDelta = -Input.GetAxis("Mouse Y") * Time.deltaTime;

        if (InvertMouse) VerticalDelta *= -1;

        HorizontalAngle += HorizontalDelta;
        VerticalAngle += VerticalDelta;

        VerticalAngle = Mathf.Clamp(VerticalAngle, -((Mathf.PI * 3f) / 4f), -(Mathf.PI / 4f));

        if ((Mathf.Abs(HorizontalDelta) + Mathf.Abs(VerticalDelta)) > 0.001f) OnCameraMove();

        ResetCameraDisplacement();

        UpdateCameraPosition();
    }

    void EndThirdPersonState()
    {

    }

    void BeginFirstPersonState()
    {

    }

    void FirstPersonStateUpdate()
    {

    }

    void EndFirstPersonState()
    {

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

    #region CameraStateConditions

    bool ChangeCamMode()
    {
        return CamModeChange;
    }

    #endregion

    void SetupStateMachine()
    {
        // Create Machine
        CameraStates = new StateMachine();

        // Create States
        State ThirdPersonState = new State();
        State FirstPersonState = new State();

        // Create Transistions
        Transition GoToFirst = new Transition();
        Transition GoToThird = new Transition();

        // Add States to Machine
        CameraStates.States.Add(ThirdPersonState);
        CameraStates.States.Add(FirstPersonState);

        // Assign Initial State
        CameraStates.InitialState = CameraStates.States[0];

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
            // not required as it is set to true by default.

        // Start the State Machine
        CameraStates.InitMachine();
    }

#endregion

}
