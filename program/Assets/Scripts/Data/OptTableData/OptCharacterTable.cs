using System.Collections.Generic;
using UnityEngine;

public class OptCharacterTable : ScriptableObject
{
    public int index;
    public EnumOptCharacterJob optCharacterJob;
    public int imageIndex;
    public List<int> availableSkillIndexList = new List<int>();
}
