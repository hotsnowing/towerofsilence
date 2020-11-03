using System.Collections.Generic;
using UnityEngine;

public class OptMapContainerTable : ScriptableObject
{
    public int index;
    public List<OptMapTable> mapDataList = new List<OptMapTable>();
}
