using UnityEngine;
using System.Collections;

public class KillTarget : Base_Mission
{

    private GameObject _Spawntarget;
    private GameObject _Target;
    public int quest;

    void Start()
    {
        MissonDetails("Kill: "+ gameObject.name, quest);
        _Target = (GameObject)Resources.Load("Target");
        spawnTarget();
    }
    void spawnTarget()
    {
        _Target = (GameObject)Instantiate(_Target, Vector3.zero, Quaternion.identity);
        _Target.transform.parent = gameObject.transform;
        _Target.transform.localPosition = new Vector3(0, 1.8f, 0);
    }
    void Update()
    {
        Completed(CheckComplete());
    }

    private bool CheckComplete()
    {
        if (GetComponent<Base_Enemy>()._state == Base_Enemy.State.Dead)
        {
            return true;
        }
        return false;
    }

    private void Completed(bool _hasCompleted)
    {
        if (_hasCompleted)
        {
            MissionComplete();
            Destroy(_Target);
            Destroy(GetComponent<KillTarget>());
        }
    }
}
