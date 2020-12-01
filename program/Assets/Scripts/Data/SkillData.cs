using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum EnumSkillType { skill }
[System.Serializable]
public class SkillDataBox//Id와 Level을 가진 컨테이너
{
    public int skill_Id;
    public int skill_Level;
    public SkillDataBox(int skill_Id, int skill_Level)
    {
        this.skill_Id = skill_Id;
        this.skill_Level = skill_Level;
    }
}
[System.Serializable]
public class SkillData//SkillData - 직업중 10개의 스킬의 레벨을 시작전 입력받음
{
    public CharacterJob characterJob;
    [Header("--Skill_Level--")]
    public int[] skill_Level;
    public int GetSkill_Level(int index)
    {
        return skill_Level[index];
    }
}
