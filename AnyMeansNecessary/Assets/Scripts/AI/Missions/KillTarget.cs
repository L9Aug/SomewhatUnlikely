using UnityEngine;
using System.Collections;

public class KillTarget : Base_Mission
{

    public GameObject enemyTarget;
    private GameObject _Target;

    void Start()
    {
        MissonDetails("Kill: "+ enemyTarget.gameObject.name, questNumber);
        _Target = (GameObject)Resources.Load("Target");
        spawnTarget();
    }
    void spawnTarget()
    {
        _Target = (GameObject)Instantiate(_Target, Vector3.zero, Quaternion.identity);
        _Target.transform.parent = enemyTarget.gameObject.transform;
        _Target.transform.localPosition = new Vector3(0, 1.8f, 0);
    }
    void Update()
    {
        Completed(CheckComplete());
    }

    private bool CheckComplete()
    {
        if (enemyTarget.GetComponent<Base_Enemy>()._state == Base_Enemy.State.Dead)
        {
            return true;
        }
        return false;
    }

    private void Completed(bool _hasCompleted)
    {
        if (_hasCompleted)
        {
            questCompletedAmount++;
            MissionComplete();
            giveXP(xpReward);
            Destroy(_Target);
            Destroy(GetComponent<KillTarget>());
        }
    }
}
