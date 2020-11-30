using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkillData : MonoBehaviour
{
    public int skill_Id;
    public Sprite skill_Icon;
    public string skill_Name;
    public int[] skill_Cost;
    public string skill_Content;
    public bool isSelectEnemy;

    public BasicSkillData(int skill_Id, Sprite skill_Icon, string skill_Name, int[] skill_Cost, string skill_Content, bool isSelectEnemy)
    {
        this.skill_Id = skill_Id;
        this.skill_Icon = skill_Icon;
        this.skill_Name = skill_Name;
        this.skill_Cost = skill_Cost;
        this.skill_Content = skill_Content;
        this.isSelectEnemy = isSelectEnemy;
    }
}
