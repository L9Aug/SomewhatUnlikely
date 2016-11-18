using UnityEngine;
using System.Collections;

public class Standard_Enemy : Base_Enemy {

    //public State _state;

    void Update()
    {
        FSM();
    }

    /*public enum State // basic FSM some states aren't in use.
    {
        Patrol,       //patrol for player
        Chase,        //chase player
        Attack,       //Attack player if siutation correct
        wasShot,      //if ai was shot      
        Dead,         //dead
        Alerted,      //used when finding bodies
        InCover,
    }
	
    public void setState(State newState)
    {
        _state = newState; // assigns new state based on value inputted.
    }*/

    private void FSM()
    {
        switch (_state)
        {
            case State.Patrol:
                GetComponent<Enemy_Chase>().checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                GetComponent<BodyDetection>().FindBodies();
                GetComponent<EnvironmentDetection>().Detection();
                GetComponent<Enemy_Patrol>().Patrol();
                break;

            case State.Chase:
                GetComponent<Enemy_Chase>().checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                GetComponent<Enemy_Chase>().Chase();
                GetComponent<Cover>().AICover();
                break;

            case State.Attack:
                GetComponent<Enemy_Chase>().checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                break;

            case State.wasShot:
                GetComponent<Enemy_Was_Shot>().WasShot();
                break;

            case State.Dead:
                break;

            case State.Alerted:
                GetComponent<BodyDetection>().FindBodies();
                GetComponent<Enemy_Chase>().checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                break;

            case State.InCover:
                GetComponent<Cover>().AICover();
                break;
        }
    }
}
