using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillTree : MonoBehaviour {


    public static int skillPoints;

    public bool intialSkill;
    public GameObject [] previousSkill;
    public Text SkillPointCounter;

    Button currentSkill;
    bool purchased;

	// Use this for initialization
	void Start () {
        currentSkill = this.gameObject.GetComponent<Button>();
        skillPoints = 999;
	}
	
	// Update is called once per frame
	void Update () {

        if (previousSkill != null)
        {
            for (int i = 0; i < previousSkill.Length; i++)
            {
                if (intialSkill == false)
                {
                    currentSkill.interactable = false;
                }
                if (previousSkill[i].GetComponent<SkillTree>().purchased == true )
                {
                   
                    currentSkill.interactable = true;

                }
            }
        }

        else
        {
            intialSkill = true;
        }
        SkillPointCounter.text = skillPoints.ToString();
	}

    public void purchaseSkill()
    {
        if (skillPoints > 0 && purchased != true)
        {
            purchased = true;
            currentSkill.image.color = Color.grey;
            skillPoints--;
        }
    }
}
