using System.Collections.Generic;

[System.Serializable]
public class OptSavedSkill
{
    public int level;
    
    /// <summary>
    /// 직접 공격시 데미지.
    /// </summary>
    public int activeAttack;
    
    /// <summary>
    /// 소유하고 있는 것으로 생기는 데미지.
    /// </summary>
    public int passiveAttack;
    
    /// <summary>
    /// 소유하고 있는 것으로 증가되는 방어력
    /// </summary>
    public int passiveDefense;
}
