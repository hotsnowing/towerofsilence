using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MapObject : MonoBehaviour
{
    public int Index { get; set; }
    [SerializeField] private TextMeshProUGUI txtStage;
    
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject rest;
    
    [SerializeField] private GameObject mark;
    [SerializeField] private Button button;
    
    private static MapObject loadedResource = null;
    public static MapObject Load(RectTransform parent)
    {
        if (loadedResource == null)
        {
            loadedResource = Resources.Load<MapObject>("MapObject");
        }

        return Instantiate(loadedResource, parent);
    }

    public void SetData(MapData data)
    {
        txtStage.text = (data.index+1).ToString();
        Index = data.index;
        monster.SetActive(data.mapType == EnumMapType.MONSTER);
        npc.SetActive(data.mapType == EnumMapType.NPC);
        rest.SetActive(data.mapType == EnumMapType.REST);

        bool interactable = GameDataManager.Instance.CurrentStage == Index + 1;
        mark.SetActive(interactable);
        button.interactable = interactable;
    }
    
    public void SetDataTemp(bool isCleared)
    {
        txtStage.text = string.Empty;

        bool isMonster = Random.Range(0, 100) > 50;
        var mapType = isMonster ? (int)EnumMapType.MONSTER : Random.Range(0, (int)EnumMapType.BOSS);

        monster.SetActive(mapType == (int)EnumMapType.MONSTER);
        npc.SetActive(mapType == (int) EnumMapType.NPC);
        rest.SetActive(mapType == (int)EnumMapType.REST);

        bool interactable = isCleared;
        mark.SetActive(interactable);
        button.interactable = interactable;
    }

    public void OnClickStage()
    {
        SceneManager.LoadScene("Ingame");
    }
}
