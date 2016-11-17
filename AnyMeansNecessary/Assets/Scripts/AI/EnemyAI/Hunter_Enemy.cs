using UnityEngine;
using System.Collections;

public class Hunter_Enemy : MonoBehaviour {

    public State _state;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        FSM();
	}


    public enum State // basic FSM some states aren't in use.
    {
        Patrol,       //patrol for player
        Chase,       //Attack player if siutation correct
        wasShot,
        InCover,
        Dead,
    }
    public void setState(State newState)
    {
        _state = newState; // assigns new state based on value inputted.
    }

    private void FSM()
    {
        switch (_state)
        {
            case State.Patrol:
                GetComponent<Enemy_Chase>().checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                GetComponent<Enemy_Patrol>().Patrol();
                break;

            case State.Chase:
                GetComponent<Enemy_Chase>().checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                GetComponent<Enemy_Chase>().Chase();
                break;

            case State.wasShot:
                GetComponent<Enemy_Was_Shot>().WasShot();
                break;

            case State.InCover:
                break;

            case State.Dead:
                break;
        }
    }
}
