using UnityEngine;
using System.Collections;

/// <summary>
/// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
/// </summary>
/// 
public class Enemy_Was_Shot : MonoBehaviour {

    NavMeshAgent Agent;
    private GameObject Player;
    private Vector3 playerLastPos;

    // Use this for initialization
    void Start () {
        Player = GameObject.Find("Player"); // for who you want the ai to chase
        Agent = GetComponent<NavMeshAgent>();
        GetComponent<HealthComp>().healthChanged.Add(HealthUpdate);
    }

    void HealthUpdate(float health, float ChangeInHealth)
    {
        Agent.speed = 1;
        playerLastPos = Player.transform.position;

        if (ChangeInHealth < 0)
        {
            if (gameObject.tag == "StandardEnemy")
            {
                GetComponent<Standard_Enemy>().setState(Standard_Enemy.State.wasShot);
            }
            else if (gameObject.tag == "Sniper")
            {
                GetComponent<Sniper_Enemy>().setState(Sniper_Enemy.State.wasShot);
            }
            else if (gameObject.tag == "ArmoredEnemy")
            {
            }
            else if (gameObject.tag == "Hunter")
            {
                GetComponent<Hunter_Enemy>().setState(Hunter_Enemy.State.Dead);
            }
        }

        if (health <= 0)
        {
            Agent.velocity = Vector3.zero; // stops ai from sliding to last set destination
            if (gameObject.tag == "StandardEnemy")
            {
                GetComponent<Standard_Enemy>().setState(Standard_Enemy.State.Dead);

            }
            else if (gameObject.tag == "Sniper")
            {
                GetComponent<Sniper_Enemy>().setState(Sniper_Enemy.State.Dead);

            }
            else if (gameObject.tag == "ArmoredEnemy")
            {

            }
            else if (gameObject.tag == "Hunter")
            {
                GetComponent<Hunter_Enemy>().setState(Hunter_Enemy.State.Dead);
            }
            GetComponent<Animator>().SetTrigger("Takedown");
        }
    }

    public void WasShot()
    {
        Vector3 distToLastPos = transform.position - playerLastPos;
        if (GetComponent<FieldOfView>().FindVisibleTargets())
        { //if player comes into view whilst going to last player pos then chase and alert other ai's
            Agent.velocity = Vector3.zero;
            if (gameObject.tag == "StandardEnemy")
            {
                GetComponent<Standard_Enemy>().setState(Standard_Enemy.State.Chase);
            }
            else if (gameObject.tag == "Sniper")
            {
                GetComponent<Sniper_Enemy>().setState(Sniper_Enemy.State.Chase);
            }
            else if (gameObject.tag == "ArmoredEnemy")
            {
            }
            else if (gameObject.tag == "Hunter")
            {
            }
        }
        else if (distToLastPos.magnitude < 2 && gameObject.tag != "Sniper") // if within x of where the player shot from and player isnt in seight then return to patroling.
        {
            GetComponent<Enemy_Chase>().checkLost(false);
        }
        else if (!GetComponent<FieldOfView>().FindVisibleTargets() && gameObject.tag != "Sniper")
        {
            Agent.SetDestination(playerLastPos);
        }

    }
}
