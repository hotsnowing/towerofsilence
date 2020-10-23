using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

public class StartingCharacter : ScriptableObject
{
    public List<CharacterContainer> characterList = new List<CharacterContainer>();

    [MenuItem("Scriptable/StartingCharacter/Create")]
    public static void Create()
    {
        var instance = ScriptableObject.CreateInstance<StartingCharacter>();
        #if UNITY_EDITOR
        AssetDatabase.CreateAsset(instance, "Assets/Resources/Scriptable/StartingCharacter.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        #endif
    }

    public static StartingCharacter Load()
    {
        return Resources.Load<StartingCharacter>("Scriptable/StartingCharacter");
    }
    
}

[System.Serializable]
public class CharacterContainer
{
    public EnumCharacterJob characterJob;
    public List<EnumSkillType> skillTypeList;
}
