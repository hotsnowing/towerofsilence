[System.Serializable]
public class SkillData
{
    #region Serializable
    public int index;
    public EnumSkillUser skillUser;
    public EnumSkillType skillType;
    public int Level;
    #endregion
}
public enum EnumSkillUser 
{ 
    Player,
    Company0,
    Company1,
    Company2,
    Company3,
    Enemy0,
    Enemy1,
}

public enum EnumSkillType
{
    Skill1,
    Skill2,
    Skill3,
    Skill4,
    Skill5,
}

