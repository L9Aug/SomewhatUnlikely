using UnityEngine;
using System.Collections;

public class Base_Enemy : MonoBehaviour 
{
	public State _state;
    public NavMeshAgent Agent;
    public static int killCount;
    void Start()
    {
        _state = State.Patrol;
        Agent = GetComponent<NavMeshAgent>();
    }
	public void setState(State newState)
	{
		_state = newState; // assigns new state based on value inputted.
	}

	public enum State // basic FSM some states aren't in use.
	{
		Patrol,       //patrol for player
		Chase,        //chase player
		Attack,       //Attack player if siutation correct
        wasShot,      //if ai was shot      
		Dead,         //dead
		Alerted,      //used when finding bodies
		InCover,
	}
}
