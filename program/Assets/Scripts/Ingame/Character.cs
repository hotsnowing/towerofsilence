using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType 
{
    None, Player, Company0, Company1, Company2, Company3, Enemy0, Enemy1
}
public enum CharacterJob 
{
    Knight, Priest, Hunter, Assassin
}
public class Character : MonoBehaviour
{
    [Header("BasicData")]
    public CharacterType characterType;
    public int maxHp;
    public float currentHp;
    public float atkPower;
    public int maxCost;
    public int curCost;
    public float height;

    [Header("CharacterState")]
    public bool isStun;
    public bool isBarrier;
    public bool isReflect;

    private void Awake()
    {
        currentHp = maxHp;
        curCost = maxCost;
    }
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
