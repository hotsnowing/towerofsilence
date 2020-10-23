
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    public int ClearStage
    {
        get { return PlayerPrefs.GetInt("GameDataManager.ClearStage", 0); }
        set{PlayerPrefs.SetInt("GameDataManager.ClearStage", value);}
    }

    public List<int> MyCharacterList = new List<int>();


}
