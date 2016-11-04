using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

    StateMachine CameraStates;

    Camera m_Camera;

    public Vector3 FirstPersonPosition;
    public float ThirdPersonCameraDistance;
    public Vector3 ThirdPersonAnchor;

    ThirdPersonMovement m_ThirdPersonMovement;

    Transform CameraRootObject;

    bool CamModeChange = false;

    float HorizontalAngle = 0;
    float VerticalAngle = -(Mathf.PI / 2);

    bool InvertMouse = false;

    float CamChangeLerpTimer = 0;
    float CamChangeLerpDir = -1;

    Vector3 ThirdPersonTargetPosition;
    Quaternion ThirdPersonTargetRotation;
    Vector3 FirstPersonTargetPosition;
    Quaternion firstPersonTargetRotation;

    bool PunishMisAlignment = true;

	// Use this for initialization
	void Start ()
    {
        m_Camera = Camera.main;
        m_ThirdPersonMovement = GetComponent<ThirdPersonMovement>();

        GameObject CameraRootObj = new GameObject("Camera Root Obj");
        CameraRootObject = CameraRootObj.transform;
        CameraRootObj.transform.SetParent(transform, false);
        CameraRootObject.localPosition = ThirdPersonAnchor;
        m_Camera.transform.SetParent(transform, false);

        SetupStateMachine();
        CamModeChange = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        TestForCamModeChange();

        CameraStates.SMUpdate();
	}

    void TestForCamModeChange()
    {
        if(Input.GetButtonDown("CamSwitch"))
        {
            CamModeChange = true;
        }
    }

    void SetCameraPosition()
    {
        float CamLerpSpeed = 5;
        CamChangeLerpTimer += Time.deltaTime * CamChangeLerpDir * CamLerpSpeed;

        CamChangeLerpTimer = Mathf.Clamp(CamChangeLerpTimer, 0, 1);

        m_Camera.transform.position = Vector3.Lerp(ThirdPersonTargetPosition, FirstPersonTargetPosition, CamChangeLerpTimer);
        m_Camera.transform.rotation = Quaternion.Lerp(ThirdPersonTargetRotation, m_Camera.transform.rotation, CamChangeLerpTimer);
    }

    void OnCameraMove()
    {

    }

    void UpdateThirdPersonCameraPosition()
    {
        ThirdPersonTargetPosition = new Vector3(
            ThirdPersonCameraDistance * Mathf.Sin(VerticalAngle) * Mathf.Sin(HorizontalAngle),
            ThirdPersonCameraDistance * Mathf.Cos(VerticalAngle),
            ThirdPersonCameraDistance * Mathf.Sin(VerticalAngle) * Mathf.Cos(HorizontalAngle)) + CameraRootObject.position;
        ThirdPersonTargetRotation = Quaternion.LookRotation(CameraRootObject.position - ThirdPersonTargetPosition);
    }

    void UpdateFirstPersonCameraPosition()
    {
        Vector3 CrouchedDisplacement = Vector3.zero;
        if (PlayerMovementController.PlayerCrouching)
        {
            CrouchedDisplacement = GetCrouchedDisplacementVector(FirstPersonPosition);
        }

        FirstPersonTargetPosition = transform.position + FirstPersonPosition + CrouchedDisplacement;
        m_Camera.GetComponent<FirstPersonHeadBob>().m_OriginalCameraPosition = m_Camera.transform.position;
    }

    Vector3 GetCrouchedDisplacementVector(Vector3 AnchorPoint)
    {
        return new Vector3(0, (transform.position.y - (transform.position.y + AnchorPoint.y)) / 2f, 0);
    }

    /// <summary>
    /// Can be toggled with 'L' atm
    /// </summary>
    void TestMotionAlignment()
    {
        if (PunishMisAlignment)
        {
            Vector3 PlayerForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 CameraForward = Vector3.Scale(m_Camera.transform.forward, new Vector3(1, 0, 1)).normalized;

            if (Vector3.Angle(PlayerForward, CameraForward) > 80)
            {
                m_ThirdPersonMovement.MoveDirectionPunishment = 0.7f;
            }
            else
            {
                m_ThirdPersonMovement.MoveDirectionPunishment = 1;
            }
        }
    }

    #region CameraStateFuctions

    void BeginThirdPersonState()
    {

    }

    void ThirdPersonStateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PunishMisAlignment = !PunishMisAlignment;
        }

        float HorizontalDelta = Input.GetAxis("Mouse X") * Time.deltaTime;
        float VerticalDelta = -Input.GetAxis("Mouse Y") * Time.deltaTime;

        if (InvertMouse) VerticalDelta *= -1;

        HorizontalAngle += HorizontalDelta;

        VerticalAngle += VerticalDelta;

        VerticalAngle = Mathf.Clamp(VerticalAngle, -((Mathf.PI * 3f) / 4f), -(Mathf.PI / 4f));

        if ((Mathf.Abs(HorizontalDelta) + Mathf.Abs(VerticalDelta)) > 0.001f) OnCameraMove();

        UpdateThirdPersonCameraPosition();
    }

    void EndThirdPersonState()
    {
        CamChangeLerpDir = 1;
    }

    void BeginFirstPersonState()
    {
        //m_Camera.transform.rotation = Quaternion.identity;
    }

    void FirstPersonStateUpdate()
    {        
        UpdateFirstPersonCameraPosition();
    }

    void EndFirstPersonState()
    {
        CamChangeLerpDir = -1;
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
        ThirdPersonState.Actions.Add(SetCameraPosition);
        ThirdPersonState.ExitActions.Add(EndThirdPersonState);
        FirstPersonState.EntryActions.Add(BeginFirstPersonState);
        FirstPersonState.Actions.Add(FirstPersonStateUpdate);
        FirstPersonState.Actions.Add(SetCameraPosition);
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
        CameraStates.PrintMessages = false;

        // Start the State Machine
        CameraStates.InitMachine();
    }

#endregion

}
