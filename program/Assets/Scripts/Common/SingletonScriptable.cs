#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public abstract class SingletonScriptableBase<T> : ScriptableObject
    where T : SingletonScriptableBase<T>{

    protected virtual void OnInitialize() {
    }

    /// <summary>
    /// example : Test/ConfigBase.asset
    /// then : Find in "Resources/Test/ConfigBase.asset"
    /// </summary>
    /// <returns></returns>
    public static string GetPathUnderResourcesFolder()
    {
        return string.Empty;
    }   

    private string GetScriptableObjectPath() {
        string path = GetPathUnderResourcesFolder();

        Debug.Assert(!string.IsNullOrEmpty(path), "GetPathUnderAssetFolder() : Empty String");
        Debug.Assert(path.Contains(".asset"), "GetPathUnderAssetFolder() must contains '.asset'");

        return path;
    }
    
    private static T __instance = null;
    public static T Instance {
        get {
            if (__instance!=null) {
                return __instance;
            }

            __instance = CreateInstance<T>();

            string path = __instance.GetScriptableObjectPath();

            __instance = Resources.Load<T>(path.Replace(".asset",string.Empty));

#if UNITY_EDITOR
            if (__instance == null) {
                CreateDirectoryAndFile(path);
                __instance = Resources.Load<T>(path.Replace(".asset",string.Empty));
            }
#endif
            
           __instance.OnInitialize();
            return __instance;
        }
    }

#if UNITY_EDITOR    
    private static void CreateDirectoryAndFile(string path) {

        string currentPath = Application.dataPath + "/Resources";
        
        string[] splitArray = path.Split('/');

        for (int i = 0; i < splitArray.Length-1; ++i) {
            currentPath += "/" + splitArray[i];

            if (System.IO.Directory.Exists(currentPath) == false) {
                System.IO.Directory.CreateDirectory(currentPath);
            }
        }

        if(System.IO.File.Exists(Application.dataPath + "/Resources/" + path)) {
            return;
        }
        
        T created = CreateInstance<T>();
        AssetDatabase.CreateAsset(created, "Assets/Resources/" + path);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
#endif
}