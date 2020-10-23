
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    public int CurrentStage
    {
        get { return PlayerPrefs.GetInt("GameDataManager.CurrentStage", 0); }
        set{PlayerPrefs.SetInt("GameDataManager.CurrentStage", value);}
    }

    // 캐릭터 데이터 구현되기 전에는 임시로 체크.
    public bool SelectedCharacter
    {
        get
        {
            return PlayerPrefs.GetInt("GameDataManager.HasStarted", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("GameDataManager.HasStarted", value == true ? 1 : 0);
        }
    }

    public List<int> MyCharacterList = new List<int>();
}
