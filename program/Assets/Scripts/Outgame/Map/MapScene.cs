using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScene : MonoBehaviour
{
    [SerializeField] private List<MapRowContainer> rowContainerList = new List<MapRowContainer>();
    private List<MapObject> mapObjectList = new List<MapObject>();

    [SerializeField] private RectTransform mapParent;
    
    void Start()
    {
        int stage = GameDataManager.Instance.CurrentStage;

        const int colCount = 8;
        bool first = true;
        for (int i = 0; i < rowContainerList.Count; ++i)
        {
            var row = rowContainerList[i];
            int spawnCount = Random.Range(1, 4);
            
            List<int> reserved = new List<int>();
            for (int j = 0; j < colCount; ++j)
            {
                reserved.Add(j);
            }

            for (int j = 0; j < spawnCount; ++j)
            {
                int index = Random.Range(0, reserved.Count);
                int value = reserved[index];
                reserved.RemoveAt(index);
                
                var mapObject = MapObject.Load(row.children[value]);
                mapObject.SetDataTemp(first);
                
                if (first)
                    first = false;
            }
        }
        
//        for (int i = 0; i < MapManager.Instance.mapDataList.Count; ++i)
//        {
//            var mapData = MapManager.Instance.mapDataList[i];
//            var mapObject = MapObject.Load(mapParent);
//            mapObjectList.Add(mapObject);
//            mapObject.SetData(mapData);
//        }
    }
}
