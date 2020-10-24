using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScene : MonoBehaviour
{
    [SerializeField] private List<MapRowContainer> rowContainerList = new List<MapRowContainer>();
    private List<MapObject> mapObjectList = new List<MapObject>();

    [SerializeField] private RectTransform mapParent;
    [SerializeField] private RectTransform lineParent;

    public List<RectTransform> test1 = new List<RectTransform>();
    public List<RectTransform> test2 = new List<RectTransform>();
    
    private HashSet<MapObject> used = new HashSet<MapObject>();
    [SerializeField]
    private RectTransform boss;
    
    IEnumerator Start()
    {
        int stage = GameDataManager.Instance.CurrentStage;

        const int colCount = 8;
        Dictionary<int, List<MapObject>> dictionary = new Dictionary<int, List<MapObject>>();
        for (int i = 0; i < rowContainerList.Count; ++i)
        {
            var row = rowContainerList[i];
            int spawnCount = Random.Range(1, 4);
            if (i == 0)
            {
                spawnCount = 1;
            }
            
            List<int> reserved = new List<int>();
            for (int j = 0; j < colCount; ++j)
            {
                reserved.Add(j);
            }
                
            int activeIndex = Random.Range(0, spawnCount);

            for (int j = 0; j < spawnCount; ++j)
            {
                int index = Random.Range(0, reserved.Count);
                int value = reserved[index];
                reserved.RemoveAt(index);
                
                var mapObject = MapObject.Load(row.children[value]);
                bool isActive = GameDataManager.Instance.CurrentStage - 1 == i && activeIndex == j;
                mapObject.SetDataTemp(isActive);

                if (dictionary.ContainsKey(i) == false)
                {
                    dictionary.Add(i, new List<MapObject>());
                }
                dictionary[i].Add(mapObject);
            }
        }

        yield return null;

        foreach (var pair in dictionary)
        {
            int current = pair.Key;
            var currentList = dictionary[current];
            
            if (false == dictionary.ContainsKey(current + 1))
            {
                for (int i = 0; i < currentList.Count; ++i)
                {
                    var currentItem = currentList[i].transform.parent.GetComponent<RectTransform>();
                    var nextItem = boss;
                
                    test1.Add(currentItem);
                    test2.Add(nextItem);
                
                    var line = ObjectPool<MapLine>.Instance.Rent(lineParent);

                    var lineRtfm = line.GetComponent<RectTransform>();

                    // sizeDelta : 그리드 피벗이 좌측상단이기때문에 조정.
                    Vector2 start = currentItem.anchoredPosition +
                                    currentItem.parent.GetComponent<RectTransform>().anchoredPosition;
                    start.x -= currentItem.sizeDelta.x * 0.5F;
                    start.y += currentItem.sizeDelta.y * 0.5F;

                    Vector2 end = nextItem.anchoredPosition +
                                  nextItem.parent.GetComponent<RectTransform>().anchoredPosition;
//                    end.x -= nextItem.sizeDelta.x * 0.5F;
//                    end.y += nextItem.sizeDelta.y * 0.5F;

                    var dir = end - start;
                    var angle = Vector2.Angle(Vector2.right, dir);

                    lineRtfm.localRotation = Quaternion.Euler(0,0,angle);
                    var sizeDelta = lineRtfm.sizeDelta;
                    sizeDelta.x = Vector2.Distance(start, end);
                    lineRtfm.sizeDelta = sizeDelta;

                    Debug.Log("current:"+start.ToString() + " / next:" + end.ToString());
                    var anchoredPos = start + (end - start) * 0.5F;
                    lineRtfm.anchoredPosition = anchoredPos;
                }
                
                continue;
            }

            var nextList = dictionary[current + 1];

            List<MapObject> prevList = null;
            if (dictionary.ContainsKey(current - 1))
            {
                prevList = dictionary[current - 1];
            }

            for (int i = 0; i < currentList.Count; ++i)
            {
                var currentItem = currentList[i].transform.parent.GetComponent<RectTransform>();
                var nextIndex = Random.Range(0, nextList.Count);
                var nextItem = nextList[nextIndex].transform.parent.GetComponent<RectTransform>();
                
                test1.Add(currentItem);
                test2.Add(nextItem);
                
                var line = ObjectPool<MapLine>.Instance.Rent(lineParent);

                var lineRtfm = line.GetComponent<RectTransform>();

                // sizeDelta : 그리드 피벗이 좌측상단이기때문에 조정.
                Vector2 start = currentItem.anchoredPosition +
                                currentItem.parent.GetComponent<RectTransform>().anchoredPosition;
                start.x -= currentItem.sizeDelta.x * 0.5F;
                start.y += currentItem.sizeDelta.y * 0.5F;

                Vector2 end = nextItem.anchoredPosition +
                              nextItem.parent.GetComponent<RectTransform>().anchoredPosition;
                end.x -= nextItem.sizeDelta.x * 0.5F;
                end.y += nextItem.sizeDelta.y * 0.5F;

                var dir = end - start;
                var angle = Vector2.Angle(Vector2.right, dir);

                lineRtfm.localRotation = Quaternion.Euler(0,0,angle);
                var sizeDelta = lineRtfm.sizeDelta;
                sizeDelta.x = Vector2.Distance(start, end);
                lineRtfm.sizeDelta = sizeDelta;

                Debug.Log("current:"+start.ToString() + " / next:" + end.ToString());
                var anchoredPos = start + (end - start) * 0.5F;
                lineRtfm.anchoredPosition = anchoredPos;
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
