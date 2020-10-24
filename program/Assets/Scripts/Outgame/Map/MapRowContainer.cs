using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class MapRowContainer : MonoBehaviour
{
    [System.NonSerialized]
    public List<RectTransform> children = new List<RectTransform>();

    private void Awake()
    {
        var cachedTransform = transform;
        int childCount = cachedTransform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            children.Add(cachedTransform.GetChild(i) as RectTransform);
        }
    }

}
