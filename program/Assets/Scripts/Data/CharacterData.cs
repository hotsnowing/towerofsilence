using System.Collections.Generic;

[System.Serializable]
public class CharacterData
{
    #region Sesrializable
    
    public int index;
    public int imageIndex;
    public EnumCharacterJob job;
    public List<int> skillIndexList = new List<int>();
    
    #endregion
    
    [System.NonSerialized]
    public List<SkillData> skillDataList = new List<SkillData>();
}

public enum EnumCharacterJob
{
    Warrior,
    Theif,
    Priest,
}

