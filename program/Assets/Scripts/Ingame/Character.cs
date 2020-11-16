﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType 
{
    Player, Company0, Company1, Company2, Company3, Enemy0, Enemy1,
}
public class Character : MonoBehaviour
{
    [Header("BasicData")]
    public CharacterType characterType;
    public int maxHp;
    public int currentHp;
    public int atkPower;
    public int maxCost;
    public float height;
    
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
