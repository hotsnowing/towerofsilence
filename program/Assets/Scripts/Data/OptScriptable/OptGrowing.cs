using UnityEngine;
using UnityEngine.Serialization;

public enum ENUM_GROWING
{
    PLUS_MAX_HP=0,
    MINUS_MAX_HP=1,
}

public class OptGrowing : ScriptableBase<OptGrowing>
{
    #region Serializable
    public ENUM_GROWING growing;
    #endregion
}
