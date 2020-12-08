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
public enum CharacterSide{ Player, Enemy}
/*
버프 디버프
      - 기절 : 1턴 동안 행동 불가
      - 중독 : 매 턴 시작 시 1의 피해, 중독 중첩 시 턴 초기화 및 피해 1 증가
      - 보호막 : 3턴 동안 유지되는 추가 체력, 회복 불가능
      - 은신 : 적의 스킬 대상으로 지정되지 않음, 광역피해는 적용됨
*/

public enum CharacterState 
{ 
    None,
    DefenseMode, //받는 피해 -2, 아군 턴 시작 시 체력 회복 3입니다.
    ReflectDefenseMode
}
public class Character : MonoBehaviour
{
    [Header("BasicData")]
    public CharacterType characterType;
    public CharacterSide characterSide;
    public float maxHp;
    public float currentHp;
    public float atkPower;
    public int maxCost;
    public int curCost;
    public float height;

    [Header("CharacterState")]
    public CharacterState characterState;

    [Header("Armor")]
    public float amountOfArmor;
    public bool isReflectDamage;

    //(체력 + 아머 개념) 다음 쉴드
    [Header("Buff & DeBuff")]
    public bool isStun;
    public bool isPoisoned;
    public bool isCloaking;
    public float maxShield;
    public float curShield = 0;

    private void Awake()
    {
        currentHp = maxHp;
        curCost = maxCost;
        characterState = CharacterState.None;
    }
    public void TakeDamage(float damage)
    {
        damage -= amountOfArmor;  //1.아머로 인한 피해감소가 첫번쨰

        float pureDamage = curShield - damage; //2. 쉴드량을 뺀 순수대미지를 구한다.
        if (pureDamage < 0) //쉴드 뚧   
        {
            curShield = 0; //쉴드 0으로 초기화
            currentHp += pureDamage;
        }
        else
            curShield -= damage;

        //죽을 경우
    }
    public void TakeHeal(float heal, bool isOverHeal)
    {
        if(!isOverHeal) //초과량 삭제
            currentHp = (currentHp + heal > maxHp) ? maxHp : (currentHp + heal);
        else
        {
            //초과량이 쉴드로 전환
            currentHp = currentHp + heal;
            if(currentHp > maxHp)
                curShield = currentHp - (currentHp = maxHp);
        }
    }
    public float getHP()
    {
        return currentHp;
    }
    public void SetSheild(float curShield)
    {
        this.curShield += curShield;
    }
    public void SetStun()
    {
        isStun = true;
    }
}
