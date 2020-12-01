using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum EnumSkillType { skill }
[System.Serializable]
public class SkillDataBox//Id�� Level�� ���� �����̳�
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
public class SkillData//SkillData - ������ 10���� ��ų�� ������ ������ �Է¹���
{
    public CharacterJob characterJob;
    [Header("--Skill_Level--")]
    public int[] skill_Level;
    public int GetSkill_Level(int index)
    {
        return skill_Level[index];
    }
}
