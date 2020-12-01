using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkillData : MonoBehaviour
{
    public int skill_Id;
    public Sprite skill_Icon;
    public string skill_Name;
    public int skill_Cost;
    public int[ , ] skill_Variable;
    public string skill_Content;
    public bool isSelectEnemy;

    public BasicSkillData(int skill_Id, Sprite skill_Icon, string skill_Name, int skill_Cost, int[ , ] skill_Variable, string skill_Content, bool isSelectEnemy)
    {
        this.skill_Id = skill_Id;
        this.skill_Icon = skill_Icon;
        this.skill_Name = skill_Name;
        this.skill_Cost = skill_Cost;
        this.skill_Variable = skill_Variable;
        this.skill_Content = skill_Content;
        this.isSelectEnemy = isSelectEnemy;
    }
    public int GetSkill_Variable(int variable_Number, int skill_Level)
    {
        return skill_Variable[variable_Number - 1, skill_Level - 1];
    }
}
