/// <summary>
/// 맵 오브젝트의 위치들을 랜덤으로 생성한다.
/// 어플리케이션 최초실행시 한 번 랜덤 돌리고, 그 이후엔 그 상태를 유지.
/// </summary>
public class MapBuilder : Singleton<MapBuilder>
{
    public MapBuilder()
    {
        for (int i = 0; i < MapManager.Instance.mapDataList.Count; ++i)
        {
            
        }
    }
}
