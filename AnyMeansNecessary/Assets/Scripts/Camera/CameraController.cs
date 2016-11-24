using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

    public SM.StateMachine CameraStates;

    Camera m_Camera;

    public Vector3 FirstPersonPosition;
    public float ThirdPersonCameraDistance;
    public Vector3 ThirdPersonAnchor;

    public UMARecipeBase FirstPersonRecipe;
    public UMARecipeBase ThirdPersonRecipe;

    private UMA.UMAData umaData;
    private UMA.SlotData[] slotData;
    private UMA.SlotData[] firstPersonSlots;

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
        umaData = GetComponent<UMA.UMAData>();

        GameObject CameraRootObj = new GameObject("Camera Root Obj");
        CameraRootObject = CameraRootObj.transform;
        CameraRootObj.transform.SetParent(transform, false);
        CameraRootObject.localPosition = ThirdPersonAnchor;
        m_Camera.transform.SetParent(transform, false);

        SetupStateMachine();
        //CamModeChange = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        CameraStates.SMUpdate();
	}

    void TestForCamModeChange()
    {
        if(Input.GetButtonDown("CamSwitch"))
        {
            if (GetComponent<PlayerMovementController>().PMSM.GetCurrentState() == "Movement" && GetComponent<PlayerController>().PSM.GetCurrentState() == "Active")
            {
                CamModeChange = true;
            }
        }
    }

    void SetCameraPosition()
    {
        float CamLerpSpeed = 5;
        CamChangeLerpTimer += Time.deltaTime * CamChangeLerpDir * CamLerpSpeed;

        CamChangeLerpTimer = Mathf.Clamp(CamChangeLerpTimer, 0, 1);

        m_Camera.transform.position = Vector3.Lerp(ThirdPersonTargetPosition, FirstPersonTargetPosition, CamChangeLerpTimer);
        //m_Camera.transform.rotation = Quaternion.Lerp(ThirdPersonTargetRotation, m_Camera.transform.rotation, CamChangeLerpTimer);
    }

    void UpdateThirdPersonCameraPosition()
    {
        ThirdPersonTargetPosition = new Vector3(
            ThirdPersonCameraDistance * Mathf.Sin(VerticalAngle) * Mathf.Sin(HorizontalAngle),
            ThirdPersonCameraDistance * Mathf.Cos(VerticalAngle),
            ThirdPersonCameraDistance * Mathf.Sin(VerticalAngle) * Mathf.Cos(HorizontalAngle)) + CameraRootObject.position;
        ThirdPersonTargetRotation = Quaternion.identity;
    }

    void UpdateFirstPersonCameraPosition()
    {
        Vector3 CrouchedDisplacement = Vector3.zero;
        if (PlayerMovementController.PlayerCrouching)
        {
            CrouchedDisplacement = GetCrouchedDisplacementVector(FirstPersonPosition);
        }

        FirstPersonTargetPosition = transform.position + FirstPersonPosition + CrouchedDisplacement;
        //m_Camera.GetComponent<FirstPersonHeadBob>().m_OriginalCameraPosition = m_Camera.transform.position;
    }

    Vector3 GetCrouchedDisplacementVector(Vector3 AnchorPoint)
    {
        return new Vector3(0, (transform.position.y - (transform.position.y + AnchorPoint.y)) / 2f, 0);
    }

    #region CameraStateFuctions

    void ThirdPersonStateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PunishMisAlignment = !PunishMisAlignment;
        }

        float HorizontalDelta = Input.GetAxis("Mouse X") * Time.deltaTime;
        float VerticalDelta = -Input.GetAxis("Mouse Y") * Time.deltaTime;

        if (InvertMouse) VerticalDelta *= -1;

        HorizontalAngle = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

        VerticalAngle = transform.rotation.eulerAngles.x * Mathf.Deg2Rad;

        VerticalAngle = Mathf.Clamp(VerticalAngle, -((Mathf.PI * 3f) / 4f), -(Mathf.PI / 4f));

        UpdateThirdPersonCameraPosition();
    }

    void EndThirdPersonState()
    {
        CamChangeLerpDir = 1;
        LoadRecipe(FirstPersonRecipe);
    }

    void FirstPersonStateUpdate()
    {        
        UpdateFirstPersonCameraPosition();
    }

    void EndFirstPersonState()
    {
        CamChangeLerpDir = -1;
        LoadRecipe(ThirdPersonRecipe);
    }

    void LoadRecipe(UMARecipeBase recipe)
    {
        GetComponent<UMADynamicAvatar>().Load(recipe);
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
        // Configure Conditions For Transitions
        Condition.BoolCondition CamSwitchCond = new Condition.BoolCondition();
        CamSwitchCond.Condition = ChangeCamMode;

        // Create Transistions
        SM.Transition GoToFirst = new SM.Transition("Go to first person", CamSwitchCond, ResetCamSwitchBool);
        SM.Transition GoToThird = new SM.Transition("Go to third person", CamSwitchCond, ResetCamSwitchBool);

        // Create States
        SM.State ThirdPersonState = new SM.State("Third person state",
            //transitions
            new List<SM.Transition>() { GoToFirst },
            //entry actions
            null,
            //update actions
            new List<SM.Action>() { TestForCamModeChange, ThirdPersonStateUpdate, SetCameraPosition },
            //exit actions
            new List<SM.Action>() { EndThirdPersonState });

        SM.State FirstPersonState = new SM.State("First person state",
            new List<SM.Transition>() { GoToThird },
            null,
            new List<SM.Action>() { TestForCamModeChange, FirstPersonStateUpdate, SetCameraPosition },
            new List<SM.Action>() { EndFirstPersonState });

        // Assign Target States to Transitions
        GoToFirst.SetTargetState(FirstPersonState);
        GoToThird.SetTargetState(ThirdPersonState);

        // Create Machine
        CameraStates = new SM.StateMachine(null, ThirdPersonState, FirstPersonState);

        // Start the State Machine
        CameraStates.InitMachine();
    }

#endregion

}
