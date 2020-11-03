using System.Collections.Generic;

[System.Serializable]
public class OptSavedCharacter
{
    public int level;
    public List<OptSavedSkill> skillList = new List<OptSavedSkill>();

    /// <summary>
    /// 캐릭터의 코스트
    /// </summary>
    public int manaCost;

    /// <summary>
    /// 캐릭터 기본 데미지
    /// </summary>
    public int defaultAttack;

    /// <summary>
    /// 캐릭터 기본 방어력
    /// </summary>
    public int defaultDefense;

    /// <summary>
    /// 캐릭터 레벨에 따른 어드밴티지가 있다면 여기에 구현.
    /// </summary>
    /// <returns></returns>
    public int GetAttackBonusByLevel()
    {
        // ex : return level * 2;
        return 0;
    }

    public int GetAttackDamage(int slotIndex)
    {
        int damage = defaultAttack;
        for (int i = 0; i < skillList.Count; ++i)
        {
            damage += skillList[i].passiveAttack;
        }

        damage += skillList[slotIndex].activeAttack;

        damage += GetAttackBonusByLevel();

        return damage;
    }

    public int GetDefenseScore()
    {
        return defaultDefense;
    }
}