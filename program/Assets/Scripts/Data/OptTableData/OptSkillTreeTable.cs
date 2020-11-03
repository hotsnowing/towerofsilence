using System.Collections.Generic;
using UnityEngine;

public class OptSkillTreeTable : ScriptableBase<OptSkillTreeTable>
{
    #if UNITY_EDITOR
    [UnityEditor.MenuItem("Scriptable/OptSkillTreeContainer")]
    public static void Create()
    {
        CreateItem("OptSkillTreeContainer");
    }
    #endif
    
    public int characterIndex;
    public List<OptSkillTable> skillDataList = new List<OptSkillTable>();
}
