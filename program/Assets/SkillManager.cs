using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkillManager : MonoBehaviour
{
    /*
       각 직업별 스킬은 10종류, 스테이지를 클리어하면 얻는 SP로 스킬 강화 및 추가 가능
       1LV 이상의 스킬 중 랜덤하게 5가지(중복 가능) 선택되어 전투UI 하단에 출현
    */
    static public SkillManager instance;
    #region singleton
    private void Awake()
    {
        //#.Scene이 넘어가도 유지
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        //#.GenerateBasicData
        Generate();
    }
    #endregion singleton
    public Sprite[] skill_Icons;
    private BasicSkillData[] basicSkillDatas;

    //#.GenerateSkillData
    private void Generate()
    {
        //| skill_Id | skill_Icon | skill_Name | skill_Cost | variable | skill_Content | isSelectEnemy
        basicSkillDatas = new BasicSkillData[40];
        //#.KnightSkill
        basicSkillDatas[0] = new BasicSkillData(0, SkillType.Attack, skill_Icons[0], "무기 강타", 1, new int[1,5] { { 1, 2, 4, 6, 8 } }, 
            " 가장 앞의 적을 가격하여 피해(변수1)를 입힙니다/3레벨 기정상태 추가 ", true);
        basicSkillDatas[1] = new BasicSkillData(1, SkillType.Defense, skill_Icons[1], "수호 방패", 1, new int[1,5] { { 2, 4, 6, 8, 10 } }, 
            " 다음 공격 스킬 사용 전 까지 보호막(변수1)을 얻고 방어태세에 들어갑니다.2LV -> 도발 효과 추가 (우선 타게팅)", false);
        basicSkillDatas[2] = new BasicSkillData(2, SkillType.Defense, skill_Icons[2], "가시 갑옷", 2, new int[1,5] { { 2, 4, 6, 8, 10 } },
            " 다음 입는 피해를 (변수) 감소시키고 반사합니다", false);
        basicSkillDatas[3] = new BasicSkillData(3, SkillType.Attack, skill_Icons[3], "붕괴", 3, new int[1, 5] { { 1, 2, 3, 4, 5 } },
            " 적 전체를 공격하여 (변수) 피해를 입히고 50%확률로 한 턴 기절 시킵니다.5LV->기절 확률 100 %", false);
        basicSkillDatas[4] = new BasicSkillData(4, SkillType.Buff, skill_Icons[4], "전투 회복", 2, new int[1, 5] { { 1, 1, 2, 2, 3 } },
            " 이번 턴, 공격 시 체력을 (변수) 회복합니다. 4LV -> 최대체력 초과 회복량은 보호막으로 치환", false);
        basicSkillDatas[5] = new BasicSkillData(5, SkillType.Attack, skill_Icons[5], "칼리번", 2, new int[2, 5] { { 3, 6, 9, 12, 15 }, { 1, 1, 1, 2, 2 } },
            " 가장 앞의 적을 가격하여 피해(변수)를 입힙니다. 마나 코스트(변수2)를 회복(1/1/1/2/2)합니다", false);
        basicSkillDatas[6] = new BasicSkillData(6, SkillType.Defense, skill_Icons[6], "아이기스", 3, new int[1, 5] { { 1, 2, 3, 4, 5 } },
            " 동료가 입는 피해(변수)를 대신 받습니다. 방어태세에 들어갑니다. 5LV -> 방어태세 전환 시 받는 피해 1 추가 감소 ", false);
        basicSkillDatas[7] = new BasicSkillData(7, SkillType.Buff, skill_Icons[7], "전투 열광", 4, new int[2, 5] { { 2, 2, 4, 4, 6 }, { 1, 1, 1, 1, 2 } },
            " 이번 턴, 다음 스킬 피해(변수1)가 증가합니다. 스킬을 생성(변수2)합니다. 정신력을 10 소모", false);
        basicSkillDatas[8] = new BasicSkillData(8, SkillType.Buff, skill_Icons[8], "기사도", 5, new int[1, 5] { { 1, 2, 3, 4, 5 } },
            " 이번 턴, 모든 아군의 스킬 피해를 (변수) 증가시킵니다. 정신력을 20 소모합니다. ", false);
        basicSkillDatas[9] = new BasicSkillData(9, SkillType.Attack, skill_Icons[9], "하늘배기", 5, new int[1, 5] { { 6, 9, 12, 15, 18 } },
            " 가장 앞의 적과 그 뒤의 적을 강하게 베어 (변수) 피해를 입힙니다. 보호막을 무시합니다 ", false);
        //#.Priest
        //#.Hunter
        //#.Assassin
    }
    public BasicSkillData GetBasicSkillData(int skill_Id)
    {
        foreach(BasicSkillData temp in basicSkillDatas)
        {
            if(temp.skill_Id == skill_Id) return temp;
        }
        Debug.LogError("Can'tFindSkill : skillManager.GetSkillCost");
        return null;
    }
    public int GetSkill_Id(CharacterJob characterJob, int index)
    {
        switch (characterJob) 
        {
            case CharacterJob.Knight:
                return 0 + index;
            case CharacterJob.Priest:
                return 10 + index;
            case CharacterJob.Hunter:
                return 20 + index;
            case CharacterJob.Assassin:
                return 30 + index;
            default: //Error
                return 0;
        }
    }

    public bool CheckSkillOption(SkillDataBox skillDataBox)
    {
        return PlaySkill(skillDataBox);
    }

    private bool PlaySkill(SkillDataBox skillDataBox)
    {
        GameObject skill_User = ChooseCharacter.instance.GetSkill_User();
        GameObject skill_Target = ChooseCharacter.instance.GetSkill_Target();
        //#.1 KnightSkill
        if (skillDataBox.skill_Id < 10)
        {
            switch (skillDataBox.skill_Id)
            {
                case 0:
                    if (skill_Target)
                    {
                        skill_User = skill_User.transform.parent.gameObject;
                        skill_Target = skill_Target.transform.parent.gameObject;
                        KnightSkill1(skill_User, skill_Target, skillDataBox);
                        return true;
                    }
                    break;
                case 1:
                    KnightSkill2(skill_User, skillDataBox);
                    return true;
            }
        }
        //#.2 PriestSkill
        else if(skillDataBox.skill_Id < 20)
        {

        }
        //#.3 HunterSkill
        else if (skillDataBox.skill_Id < 30)
        {

        }
        //#.4 AssassinSkill
        else if (skillDataBox.skill_Id < 40)
        {

        }
        return false;
    }
    private void CheckUser_State(Character user)
    {
        switch (user.characterState) 
        {
            case CharacterState.DefenseMode:
                user.characterState = CharacterState.None;
                break;
            default:
                break;
        }
    }
    //#.KnightSkill
    //무기강타 | 가장 앞의 적을 가격하여 피해를 (변수) 입힙니다. | 1/2/4/6/8 | 3LV -> 기절 상태이상 추가 | 1
    private void KnightSkill1(GameObject skill_User, GameObject skill_Target, SkillDataBox skillDataBox)
    {
        Character temp_User = skill_User.GetComponent<Character>();

        CheckUser_State(temp_User);

        skill_Target.GetComponent<Character>().TakeDamage(
            basicSkillDatas[skillDataBox.skill_Id].GetSkill_Variable(1, skillDataBox.skill_Level));
    }
    //수호 방패 | 다음 공격 스킬 사용 전 까지 보호막을(변수) 얻고 방어태세에 들어갑니다. | 2/4/6/8/10 | 2LV -> 도발 효과 추가(우선 타게팅) | 1
    private void KnightSkill2(GameObject skill_User, SkillDataBox skillDataBox)
    {
        Character temp_User = skill_User.GetComponent<Character>();

        CheckUser_State(temp_User);

        temp_User.SetSheild(
            GetBasicSkillData(skillDataBox.skill_Id)
            .GetSkill_Variable(1, skillDataBox.skill_Level));
    }
    //가시 갑옷 | 다음 입는 피해를(변수) 감소시키고 반사합니다. | 2/4/6/8/10 | 비고 없음. | 2
    private void KnightSkill3(GameObject skill_User, SkillDataBox skillDataBox)
    {
        Character temp_User = skill_User.GetComponent<Character>();

        CheckUser_State(temp_User);

        temp_User.isReflectDamage = true;
        temp_User.amountOfArmor = GetBasicSkillData(skillDataBox.skill_Id).GetSkill_Variable(1, skillDataBox.skill_Level);
    }
    //붕괴 | 적 전체를 공격하여 (변수) 피해를 입히고 50%확률로 한 턴 기절 시킵니다. | 1/2/3/4/5 | 5LV -> 기절 확률 100% | 3
    private void KnightSkill4(GameObject skill_User, SkillDataBox skillDataBox)
    {
        Character temp_User = skill_User.GetComponent<Character>();

        CheckUser_State(temp_User);
        //#.1 적전체 공격
        List<GameObject> enemyTCharacter = 
            GameObject.FindGameObjectWithTag("BattleSystem").
            GetComponent<BattleSystem>().GetEnemyTObjectList();

        float damage = GetBasicSkillData(skillDataBox.skill_Id).GetSkill_Variable(1, skillDataBox.skill_Level);
        foreach(GameObject go in enemyTCharacter)
        {
            go.GetComponent<Character>().TakeDamage(damage);
            //#.2 50%확률로 기절
            int rand = Random.Range(0, 100);
            if(rand < 50) go.GetComponent<Character>().SetStun();
        }
    }
    //전투 회복 | 이번 턴, 공격 시 체력을 (변수) 회복합니다. | 1/1/2/2/3 | 4LV -> 최대체력 초과 회복량은 보호막으로 치환 | 2
    private void KnightSkill5(GameObject skill_User, SkillDataBox skillDataBox)
    {
        Character temp_User = skill_User.GetComponent<Character>();

        CheckUser_State(temp_User);

        //#.1 heal level4 이상이면 초과량은 쉴드로 전환
        temp_User.TakeHeal(GetBasicSkillData(skillDataBox.skill_Id).GetSkill_Variable(1, skillDataBox.skill_Level), skillDataBox.skill_Level >= 4);
    }
    //칼리번 | 가장 앞의 적을 가격하여 피해를 (변수) 입힙니다. 마나 코스트를 (변수2) 회복합니다.| 3/6/9/12/15 | 1/1/1/2/2 | 비고 없음. | 2
    private void KnightSkill6(GameObject skill_User, SkillDataBox skillDataBox)
    {
        BattleSystem battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();

        //#.1 가장앞에있는 오브젝트 알아냄
        Character firstObject = skill_User.GetComponent<Character>().characterSide == CharacterSide.Player ?
            battleSystem.GetPlayerTObjectList()[0].GetComponent<Character>() : 
            battleSystem.GetEnemyTObjectList()[0].GetComponent<Character>();
        //#.2 가격
        firstObject.TakeDamage(GetBasicSkillData(skillDataBox.skill_Id).GetSkill_Variable(1, skillDataBox.skill_Level));
        //#.3 미나코스트 회복
        battleSystem.Plus_Cost(GetBasicSkillData(skillDataBox.skill_Id).GetSkill_Variable(2, skillDataBox.skill_Level), skill_User.GetComponent<Character>().characterSide);
    }
    //아이기스 | 동료가 입는 피해를 (변수) 대신 받습니다.방어태세에 들어갑니다. | 1/2/3/4/5 | 5LV -> 방어태세 전환 시 받는 피해 1 추가 감소 | 3

    //전투 열광 | 이번 턴, 다음 스킬 피해가 (변수) 증가합니다. 스킬을 (변수2) 생성합니다. 정신력을 10 소모 | 2/2/4/4/6 | 1/1/1/1/2 | 비고 없음. | 4

    //기사도 | 이번 턴, 모든 아군의 스킬 피해를 (변수) 증가시킵니다. 정신력을 20 소모합니다. | 1/2/3/4/5 | 비고 없음. | 5

    //하늘베기 | 가장 앞의 적과 그 뒤의 적을 강하게 베어 (변수) 피해를 입힙니다.보호막을 무시합니다. | 6/9/12/15/18 | 비고 없음. | 5

}
