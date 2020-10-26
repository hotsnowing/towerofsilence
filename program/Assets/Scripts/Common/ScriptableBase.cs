using UnityEngine;

public class ScriptableBase<T> : ScriptableObject where T:ScriptableBase<T>
{
    #if UNITY_EDITOR
    public static void CreateItem(string itemName)
    {
        var instance = ScriptableObject.CreateInstance<T>();
        
        UnityEditor.AssetDatabase.CreateAsset(instance, "Assets/Resources/Scriptable/"+ itemName +".asset");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }
    #endif
}
