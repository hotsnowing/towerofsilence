using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType 
{
    Player, Company0, Company1, Company2, Company3, Enemy0, Enemy1
}
public class Character : MonoBehaviour
{
    [Header("BasicData")]
    public CharacterType characterType;
    public int maxHp;
    public float currentHp;
    public float atkPower;
    public int maxCost;
    public float height;
    
    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        //#.죽을 경우*
    }

    public float getHP()
    {
        return currentHp;
    }
}
