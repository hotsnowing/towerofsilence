using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skills { EMPTY, BASICATTACK }

public class Character : MonoBehaviour
{
    public int maxHp;
    public int currentHp;

    public int atkPower;

    public int maxCost;


    //public void useSkill(Character target, int skillnumber)
    //{
            //스킬 사용 시 필요
    //}

    public bool TakeDamage(int dmg)
    {
        currentHp -= dmg;
        //죽으면 true를 반환
        if (currentHp <= 0)
            return true;
        else
            return false;
    }

    public int getHP()
    {
        return currentHp;
    }
    
}
