using UnityEngine;
using System.Collections;


/// <summary>
/// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
/// </summary>


public class AI_Main : MonoBehaviour {
    public static bool detected = false; // used for informing all enemy ai's if player is detected
    public GameObject Player; // for determining whom to chase // may be modifed at later date.
    public State _state; 
    private NavMeshAgent Agent;


    //patrol variables//
    public Transform[] Waypoints; // use empty game objects in the inpector to set new waypoints
    public int currentWaypoint;

    private Vector3 MoveDirection;
    private Vector3 Target;
    private bool reverse;
    private float lostTimer;


    private Camera PlayerCam;

    public void Start()
    {
    //    _state = State.Initalize; // nothing done in initalize, added for future purposes
        Agent = GetComponent<NavMeshAgent>();
        GetComponent<HealthComp>().healthChanged.Add(HealthUpdate);
    }
    
	// Update is called once per frame
	void Update ()
    {
        FSM();
    }

    void HealthUpdate(float health)
    {
        if(health <= 0)
        {
            setState(State.Dead);
            GetComponent<Animator>().SetTrigger("Takedown");
        }
    }

    private void FSM()
    {
        switch (_state)
        {
            case State.Initalize:
                setState(State.Patrol);
                break;

            case State.Patrol:
                checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                GetComponent<BodyDetection>().FindBodies();
                GetComponent<EnvironmentDetection>().Detection();
                Patrol();
                break;

            case State.Chase:
                checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                Chase(Player);
                GetComponent<Cover>().AICover();
                break;

            case State.Attack:
                checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                break;

            case State.Dead:
              //  kill(); // destroys gameobject
                break;
            case State.Alerted:
                GetComponent<BodyDetection>().FindBodies();
                checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                break;
            case State.InCover:
                GetComponent<Cover>().AICover();
                break;
            case State.Knockedout:
                setState(State.Patrol);
                break;
        }
    }


    public enum State // basic FSM some states aren't in use.
    {
        Initalize,    //set up any base values or from load files etc
        Patrol,       //patrol for player
        Chase,        //chase player
        Attack,       //Attack player if siutation correct
        Dead,         //dead
        Alerted,      //used when finding bodies
        InCover,
        Knockedout    //knocked out by various means
    }

    public void setState(State newState)
    {
        _state = newState; // assigns new state based on value inputted.
    }

    private void Patrol()
    {

        Agent.speed = 0.5f; // 0.5f = walking speed

        Target = Waypoints[currentWaypoint].position;
        MoveDirection = Target - Agent.transform.position;
        if (detected == true)// chases if any ai detected the player
        {
            setState(State.Chase);
        }else if (!GetComponent<EnvironmentDetection>().Detection())
        {
            if (MoveDirection.magnitude < 2)
            {
                ///determing if at end of patrol route or start then sending it in reverse direction
                if (reverse == false)
                {

                    if(Waypoints.Length > 1) { 
                        currentWaypoint++;
                    if (currentWaypoint == Waypoints.Length - 1)
                        reverse = true;
                    }
                }
                else
                {
                    currentWaypoint--;
                    if (currentWaypoint == 0)
                        reverse = false;
                }
            }
            else
            {
                Agent.SetDestination(Waypoints[currentWaypoint].position);
            }
        }   
    }

    public void checkLost( bool found) // called from fieldofview script //basic lost function with a 3 second timer
    {
        if(found)
        {
            lostTimer = 0.0f;
            detected = true;
        }
        else
        {
            lostTimer += Time.deltaTime;
        }

        if (lostTimer >= 3)
        {
            setState(State.Patrol);
            detected = false; //alerts all ai that player has been lost
            lostTimer = 0.0f;
        }
    }

    private void Chase(GameObject Player)
    {
        Agent.speed = 1;
        gameObject.transform.LookAt(Player.transform); //rotate and face player 
        Vector3 distToPlayer = transform.position - Player.transform.position;
        if (distToPlayer.magnitude < 5) // stops ai running straight into the players face.
        {
            Agent.speed = 0;
        }
       else
        {
            Agent.speed = 1;
            Agent.SetDestination(Player.transform.position); // pathfind to player with slight offset otherwise constantly pushes player

        }


    }
    private void kill()
    {
        Destroy(gameObject); //removes enemy from scene until we have a proper death.
    }
}