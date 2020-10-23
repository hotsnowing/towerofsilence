
using System.Collections.Generic;

public class MapDataManager:Singleton<MapDataManager>
{
    public enum EnumMapType
    {
        NORMAL_MONSTER=0,
        NPC=1,
        REST=2,
        BOSS=3,
    }

    public List<EnumMapType> mapTypeList = new List<EnumMapType>();

    public MapDataManager()
    {
        // 프로토타입에서 정해진 스테이지만 있게 한다.
        for (int i = 0; i < 20; ++i)
        {
            mapTypeList.Add((EnumMapType)(i%4));
        } 
    }
}
