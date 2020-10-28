using System.Collections.Generic;
using UnityEngine;

public enum EnumOptCharacterIndex
{
    SWOARDMAN_NORMAL=0,
    
    PRIEST_NORMAL = 100,
}

public enum EnumOptCharacterJob
{
    SWOARDMAN = 0,
    PRIEST = 1,
}

public class OptCharacterData : ScriptableObject
{
    #region Serializable
    public EnumOptCharacterIndex optCharacterIndex;
    public EnumOptCharacterJob optCharacterJob;
    public int imageIndex;
    #endregion

    [System.NonSerialized]
    public List<OptSkillData> skillData = new List<OptSkillData>();
    [System.NonSerialized]
    public List<OptGrowing> growingList = new List<OptGrowing>();
}
