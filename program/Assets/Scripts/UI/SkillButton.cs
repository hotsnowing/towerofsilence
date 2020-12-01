using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public int index; //현재 위치index
    public Text text;
    [Header("ButtonSavedSkillData")]
    public SkillDataBox skillDataBox;

    //#.Test
    public void SetText()   
    {
        BasicSkillData basicSD = SkillManager.instance.GetBasicSkillData(skillDataBox.skill_Id);
        text.text = (basicSD.skill_Name).ToString() + "\nCost:" + (basicSD.skill_Cost).ToString();
    }
}
