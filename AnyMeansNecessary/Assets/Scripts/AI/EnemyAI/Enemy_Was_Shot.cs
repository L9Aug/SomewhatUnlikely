using UnityEngine;
using System.Collections;

    /// <summary>
    /// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
    /// </summary>
    /// 
public class Enemy_Was_Shot : MonoBehaviour {
    private GameObject canvas;
    NavMeshAgent Agent;
    private GameObject Player;
    private Vector3 playerLastPos;

    // Use this for initialization
    void Start () {
        canvas = GameObject.Find("mainCanvas");
        Player = GameObject.Find("Player"); // for who you want the ai to chase
        Agent = GetComponent<NavMeshAgent>();
        GetComponent<HealthComp>().healthChanged.Add(HealthUpdate);
    }


    void HealthUpdate(float health, float ChangeInHealth)
    {
        if (health <= 0)
        {
            canvas.GetComponent<UIElements>().xpGain(15);
            Base_Enemy.killCount++;
            Agent.velocity = Vector3.zero; // stops ai from sliding to last set destination
            if (gameObject.tag == "StandardEnemy")
            {
                GetComponent<Standard_Enemy>().setState(Standard_Enemy.State.Dead);

            }
            else if (gameObject.tag == "Sniper")
            {
                GetComponent<Sniper_Enemy>().setState(Base_Enemy.State.Dead);
            }
            else if (gameObject.tag == "ArmoredEnemy")
            {

            }
            else if (gameObject.tag == "Hunter")
            {
                GetComponent<Hunter_Enemy>().setState(Base_Enemy.State.Dead);
            }
            GetComponent<Animator>().SetTrigger("Takedown");
        }
    }

    public void Shot()
    {
        Agent.speed = 1;
        playerLastPos = Player.transform.position;
        if (gameObject.tag == "StandardEnemy")
        {
            GetComponent<Standard_Enemy>().setState(Base_Enemy.State.wasShot);
        }
        else if (gameObject.tag == "Sniper")
        {
            GetComponent<Sniper_Enemy>().setState(Base_Enemy.State.wasShot);
        }
        else if (gameObject.tag == "ArmoredEnemy")
        {
        }
        else if (gameObject.tag == "Hunter")
        {
            GetComponent<Hunter_Enemy>().setState(Base_Enemy.State.wasShot);
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
                GetComponent<Standard_Enemy>().setState(Base_Enemy.State.Chase);
            }
            else if (gameObject.tag == "Sniper")
            {
                GetComponent<Sniper_Enemy>().setState(Base_Enemy.State.Chase);
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
