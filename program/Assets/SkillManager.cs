using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public bool CheckSkillOption(SkillData skillData)
    {
        return PlaySkill(skillData);
    }

    private bool PlaySkill(SkillData skillData)
    {
        switch (skillData.skillUser) 
        {
            case EnumSkillUser.Player:
                switch (skillData.skillType)
                {
                    case EnumSkillType.Skill1:
                        GameObject pT = ChooseCharacter.instance.playerT0;
                        GameObject eT = ChooseCharacter.instance.enemyT0;
                        if(pT && eT)
                        {
                            pT = pT.transform.parent.gameObject;
                            eT = eT.transform.parent.gameObject;
                            PlayerSkill1(pT, eT, skillData.Level);
                            return true;
                        }
                        break;
                }
                break;
        }
        return false;
    }
    //#.PlayerSkill
    private void PlayerSkill1(GameObject p, GameObject e, float level)
    {
        float skillPlusDamage = 5 * level;
        e.GetComponent<Character>().TakeDamage(p.GetComponent<Character>().atkPower + skillPlusDamage);
    }
}
