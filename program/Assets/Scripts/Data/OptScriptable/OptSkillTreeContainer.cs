using System.Collections.Generic;
using UnityEngine;

public class OptSkillTreeContainer : ScriptableBase<OptSkillTreeContainer>
{
    #if UNITY_EDITOR
    [UnityEditor.MenuItem("Scriptable/OptSkillTreeContainer")]
    public static void Create()
    {
        CreateItem("OptSkillTreeContainer");
    }
    #endif
    
    public EnumOptCharacterIndex characterIndex;
    public List<OptSkillData> skillDataList = new List<OptSkillData>();
}
