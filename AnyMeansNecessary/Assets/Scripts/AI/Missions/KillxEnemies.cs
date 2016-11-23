using UnityEngine;
using System.Collections;

public class KillxEnemies : Base_Mission
{
    /// <summary>
    /// //////////UNFINISHED
    /// </summary>

    public int _targetAmount;
    private int _remainingTargets;
	// Use this for initialization
    
	void Start () {
        _remainingTargets = Base_Enemy.killCount + _targetAmount;
	}
	
	// Update is called once per frame
	void Update () {
      //  Debug.Log(_remainingTargets);
	}

    private bool CheckComplete()
    {
        if(_remainingTargets == 0)
        {
            return true;
        }
        return false;
    }

    private void Completed(bool _hasCompleted)
    {
        if (_hasCompleted)
        {
            GetComponent<UIElements>().mainObjective.isOn = _hasCompleted;
            Destroy(GetComponent<KillxEnemies>());
        }
    }
}
