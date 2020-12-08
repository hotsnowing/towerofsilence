using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST }
public enum PlayerTurnState {WaitForTurn, SelectCharacter, SelectSkillButton, SelectEnemy, CheckSkillOption}
//JsonFile**********************************
/*
 * 오늘 할 거 
 * 캐릭터 코스트, 몹 보스 코스트 관리
 * 
 */
[System.Serializable]
public class SpawnData
{
    public List<CharacterSaveData> playerTSD;
    public List<CharacterSaveData> enemyTSD;
}
[System.Serializable]
public class CharacterSaveData
{
    public CharacterType cType;
    public CharacterJob cJob;
    //#.SkillData
    public SkillData skillData;
}
//******************************************
public class CompanyBox
{
    public GameObject character;
    public GameObject skillPackage;
    public CompanyBox(GameObject character, GameObject skillPk)
    {
        this.character = character;
        skillPackage = skillPk;
    }
}
public class BattleSystem : MonoBehaviour
{
    [Header("CharacterSpawnPosition")]
    [SerializeField] private Vector3 playerFrontSpawnPos;
    [SerializeField] private Vector3 playerTSpawnDistance;
    [SerializeField] private Vector3 playerTHidePos;
    [SerializeField] private Vector3 enemyFrontSpawnPos;
    [SerializeField] private Vector3 enemyTSpawnDistance;
    [SerializeField] private Vector3 enemyTHidePos;
    [SerializeField] private float spawnDelayTime;
    private float pFixedMoveDistance;
    private float eFixedMoveDistance;
    //#.CharacterList
    private List<CompanyBox> playerTList;
    private List<GameObject> enemyTList;
    [Header("CompositCost")]
    public int playerTCompositeCost = 0;
    public int enemyTCompositeCost = 0;
    private int minCost = 100;
    [Header("MoveData")]
    [SerializeField] private float objMoveTime;
    [SerializeField] private SpawnData spawnData; //LoadFormJson **************

    [Header("Prefab")]
    //#.PlayerT
    public GameObject playerPrefab;
    public GameObject company0Prefab;
    public GameObject company1Prefab;
    public GameObject company2Prefab;
    public GameObject company3Prefab;
    //#.EnemyT
    public GameObject enemy0Prefab;
    public GameObject enemy1Prefab;
    [Header("UI")]
    public GameObject hpBarPrefab;
    public GameObject skillPakagePrefab;
    //#.
    Character playerLogic;
    Character company0Logic;
    Character company1Logic;
    Character enemy0Logic;
    Character enemy1Logic;

    [Header("State")]
    public Text turnState;
    public BattleState battleState;
    public PlayerTurnState playerTurnState;

    [Header("Canvas")]
    public GameObject mainCanvas;
    public GameObject mainToolBarCanvas;
    public GameObject hpBarCanvas;

    //#.
    private GameObject playerObj;


    private void Awake()
    {
        playerTList = new List<CompanyBox>(); //인게임내 플레이어팀을 저장 
        enemyTList = new List<GameObject>();  //인게임내 적팀을 저장

        pFixedMoveDistance = Mathf.Abs(playerFrontSpawnPos.x - playerTHidePos.x);
        eFixedMoveDistance = Mathf.Abs(enemyTHidePos.x - enemyFrontSpawnPos.x);
    }
    void Start()
    {
        battleState = BattleState.START;
        playerTurnState = PlayerTurnState.SelectCharacter;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    { 
        load(); //Load JsonDataFile
        StartCoroutine(SpawnPlayerT());
        StartCoroutine(SpawnEnemyT());

        //#.플레이어 턴부터 시작
        StartCoroutine(PlayPlayerTurn());
        yield break;
    }

    //#.딜레이를 주기위해
    IEnumerator PlayPlayerTurn()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(PlayerTurn());
    }
    IEnumerator PlayEnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(EnemyTurn());
    }



    IEnumerator EnemyTurn()
    {
        battleState = BattleState.ENEMYTURN; //SetState
        turnState.text = battleState.ToString();
        //구현 - enemy 공격

        //임시
        StartCoroutine(PlayPlayerTurn());
        yield break;
    }

    IEnumerator EndBattle()
    {
        if (battleState == BattleState.WON)
        {
            turnState.text = "YOU WIN";
        }
        else
        {

        }
        yield return new WaitForSeconds(2f);

        //?
        ++GameDataManager.Instance.CurrentStage;
        
        SceneManager.LoadScene("Map");
    }

    //#.플레이어턴 조율
    [Header("CompromisePlayerTurn")]
    public bool isCharacterChanged;
    public bool isSkillChanged;
    private IEnumerator selectPlayerTCo;
    private IEnumerator selectSkillCo;
    private IEnumerator checkSkillOptionCo;
    private bool canChooseSkill = false;
    public CharacterType nowChoosen = CharacterType.None;
    public CharacterType lastChoosen = CharacterType.None;
    public bool ischeckingSkillOption = false;
    private SkillButton lastSkillButton;
    IEnumerator PlayerTurn()
    {
        battleState = BattleState.PLAYERTURN; //SetState

        turnState.text = battleState.ToString();

        StartCoroutine(selectPlayerTCo = SelectPlayerT());
        yield break;
    }
    IEnumerator SelectPlayerT()
    {
        playerTurnState = PlayerTurnState.SelectCharacter;
        ChooseCharacter.instance.canChoosePlayerT = true; //선택ON
        while (true)
        {
            //#.캐릭터 선택 변경
            if(nowChoosen != lastChoosen)
            {
                foreach(CompanyBox companyBox in playerTList)
                {
                    if(companyBox.character.GetComponent<Character>().characterType == nowChoosen)
                    {
                        if(!canChooseSkill) StartCoroutine(selectSkillCo = SelectSkill()); //첫선택시
                        companyBox.skillPackage.transform.SetAsLastSibling();
                        break;
                    }
                }
                lastChoosen = nowChoosen;
                isCharacterChanged = true; //선택된 캐릭터가 변경되었음을 선언
            }
            yield return null;
        }
    }
    IEnumerator SelectSkill()
    {
        playerTurnState = PlayerTurnState.SelectSkillButton;
        while (true)
        {
            canChooseSkill = true; //스킬선택 가능
            if (isCharacterChanged && playerTurnState == PlayerTurnState.SelectSkillButton) //캐릭터 변경됨
                isCharacterChanged = false;
            yield return null;
        }
    }
    IEnumerator CheckSkillOption(SkillPackage skillPackage, SkillButton skillButton, SkillDataBox skillDataBox)
    {
        playerTurnState = PlayerTurnState.CheckSkillOption;
        if (SkillManager.instance.GetBasicSkillData(skillDataBox.skill_Id).isSelectEnemy)
        {
            //#.적
            ChooseCharacter.instance.canChooseEnemyT = true;
        }
        while (true)
        {
            ischeckingSkillOption = true;
            if (isCharacterChanged)
            {
                lastSkillButton.GetComponent<Animator>().SetTrigger("Pressed");
                ischeckingSkillOption = false;
                isCharacterChanged = false;
                playerTurnState = PlayerTurnState.SelectSkillButton;
                canChooseSkill = false;
                yield break;
            }
            //#.스킬에 맞게 조건충족됬는지 체크
            if (SkillManager.instance.CheckSkillOption(skillDataBox))
            {
                ischeckingSkillOption = false;
                playerTurnState = PlayerTurnState.SelectSkillButton;
                //#.조건충족
                skillPackage.SortButtons(skillButton); //버튼 없에기
                //#.CostCheck
                playerTCompositeCost -= SkillManager.instance.GetBasicSkillData(skillDataBox.skill_Id).skill_Cost;
                yield return null;
                if (CheckSkillCost())
                {
                    playerTurnState = PlayerTurnState.SelectCharacter;
                    StopCoroutine(selectSkillCo);
                    ChooseCharacter.instance.ResetAllTarget();
                    canChooseSkill = false;
                    yield break;
                }
                else
                {
                    //#.StopCoroutine
                    StopCoroutine(selectPlayerTCo);
                    ChooseCharacter.instance.canChoosePlayerT = false;
                    ChooseCharacter.instance.canChooseEnemyT = false;
                    StopCoroutine(selectSkillCo);
                    playerTurnState = PlayerTurnState.WaitForTurn;
                    yield break;
                }
            }
            yield return null;
        }
    }
    private bool CheckSkillCost()
    {
        int temp = 0;
        foreach (CompanyBox companyBox in playerTList)
        {
            temp = companyBox.skillPackage.GetComponent<SkillPackage>().minCost;
            if (minCost >= temp) minCost = temp;
        }
        return minCost >= temp;
    }
    public void ActivateSkill(SkillPackage skillPackage, SkillButton skillButton)
    {
        //#.플레이어턴에 눌러야함 || 스킬을 선택가능할 때
        if (battleState != BattleState.PLAYERTURN || !canChooseSkill) return;
        if (ischeckingSkillOption)
        {
            isSkillChanged = true;
            lastSkillButton.GetComponent<Animator>().SetTrigger("Pressed");
            StopCoroutine(checkSkillOptionCo);
            ChooseCharacter.instance.ResetEnemyTarget();
            lastSkillButton = skillButton;
        }
        if (!lastSkillButton)
        {
            isCharacterChanged = false; //스킬버튼이 첫선택이라면 캐릭터 변경이 무의미하다.
            lastSkillButton = skillButton;
        } 
        //#.Check
        StartCoroutine(checkSkillOptionCo = CheckSkillOption(skillPackage, skillButton, 
            new SkillDataBox(skillButton.skillDataBox.skill_Id, skillButton.skillDataBox.skill_Level)));
    }
    //#.SpawnSystem
    IEnumerator SpawnPlayerT()
    { 
        Vector3 nowSpawnPos = playerFrontSpawnPos;
        List<CharacterSaveData> pT = spawnData.playerTSD;
        //#.canvas - SkillPackage를 찾음
        //#.Spawn
        for (int i=0; i<pT.Count; i++)
        {
            //#.스킬패키지 스폰 & canvas - SkillPackage 에 자식으로 설정해준다.
            GameObject skillPackage = Instantiate(skillPakagePrefab);
            skillPackage.transform.SetParent(mainToolBarCanvas.transform.GetChild(0).transform);
            skillPackage.GetComponent<RectTransform>().localPosition = new Vector3(-200, -20, 0); //스폰좌표
            skillPackage.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); //사이즈
            skillPackage.GetComponent<SkillPackage>().battleSystem = this; //프리펩에 객체 넣어주기
            SkillPackage tempSkill_Pakage = skillPackage.GetComponent<SkillPackage>();
            //#.SpawnData로 부터 해당 인덱스의 스킬데이터를 받아옴 -> 스킬패키지 내용 구성
            SkillData tempSkill_Data = spawnData.playerTSD[i].skillData;
            //#.CompanyBox에 오브젝트와 스킬페키지를 넣는다.
            GameObject characterObj = MakeObj(pT[i].cType);
            characterObj.GetComponent<Character>().characterSide = CharacterSide.Player;
            //#.Composite Cost
            Plus_Cost(characterObj.GetComponent<Character>().curCost, CharacterSide.Player);
            CompanyBox companyBox = new CompanyBox(characterObj, skillPackage);
            if(companyBox.character.tag == "Player")
                playerObj = companyBox.character;
            //#.스킬은 총5개를 랜덤으로 생성
            List<int> random_List = new List<int>();
            List<SkillDataBox> transSkill_DataBox = new List<SkillDataBox>();
            for (int j=0; j<5; j++)
            {
                //******************추후수정가능 10개중 랜덤 생성
                random_List.Add(Random.Range(0, 10));
            }
            foreach(int tempSkill_Index in random_List)
            {
                int tempSkill_Id = SkillManager.instance.GetSkill_Id(tempSkill_Data.characterJob, tempSkill_Index);
                int tempCost = SkillManager.instance.GetBasicSkillData(tempSkill_Id).skill_Cost;
                if (tempCost <= minCost)
                    minCost = tempCost;
                transSkill_DataBox.Add(new SkillDataBox(tempSkill_Id, tempSkill_Data.GetSkill_Level(tempSkill_Index)));
            }
            //#.스킬데이터 전달.
            tempSkill_Pakage.spawnSkill_DataBox = transSkill_DataBox;
            //#.스킬패키지 작동
            tempSkill_Pakage.StartSkillPackage();
            //#.안보이는 위치로 이동
            companyBox.character.transform.position = playerTHidePos;
            //#.플레이어 리스트에 추가
            playerTList.Add(companyBox);
        }
        //#.차례대로 정렬
        foreach (CompanyBox pTobj in playerTList)
        {
            StartCoroutine(MoveObject(pTobj.character, nowSpawnPos, pFixedMoveDistance));
            nowSpawnPos += playerTSpawnDistance; 
            yield return new WaitForSeconds(spawnDelayTime); //SpawnDelay
        }
    }
    IEnumerator SpawnEnemyT()
    {
        Vector3 nowSpawnPos = enemyFrontSpawnPos;
        List<CharacterSaveData> pT = spawnData.enemyTSD;
        //#.Spawn
        for(int i=0; i<pT.Count; i++)
        {
            GameObject enemyObject = MakeObj(pT[i].cType);
            enemyObject.GetComponent<Character>().characterSide = CharacterSide.Enemy;
            Plus_Cost(enemyObject.GetComponent<Character>().curCost, CharacterSide.Enemy);
            enemyObject.transform.position = enemyTHidePos;
            enemyTList.Add(enemyObject);
        }
        //#.Sort
        foreach(GameObject pTobj in enemyTList)
        {
            StartCoroutine(MoveObject(pTobj, nowSpawnPos, eFixedMoveDistance));
            nowSpawnPos += enemyTSpawnDistance; 
            yield return new WaitForSeconds(spawnDelayTime); //SpawnDelay
        }
    }
    IEnumerator MoveObject(GameObject moveObj, Vector3 targetPos, float fixedD)
    {
        float t = 0;
        float objMoveTimeT = (1 / objMoveTime) * fixedD / (Mathf.Abs(targetPos.x - moveObj.transform.position.x));
        Vector3 orgPos = moveObj.transform.position;
        while(t < 1)
        {
            moveObj.transform.position = Vector3.Lerp(orgPos, targetPos, DecreaseFunc(t));
            t += Time.deltaTime * objMoveTimeT; 
            yield return null;
        }
    }
    private GameObject MakeObj(CharacterType characterType)
    {
        GameObject go = null;
        switch (characterType)
        {
            case CharacterType.Player:
                go = Instantiate(playerPrefab);
                break;
            case CharacterType.Company0:
                go = Instantiate(company0Prefab);
                break;
            case CharacterType.Company1:
                go = Instantiate(company1Prefab);
                break;
            case CharacterType.Company2:
                go = Instantiate(company2Prefab);
                break;
            case CharacterType.Company3:
                go = Instantiate(company3Prefab);
                break;
            case CharacterType.Enemy0:
                go = Instantiate(enemy0Prefab);
                break;
            case CharacterType.Enemy1:
                go = Instantiate(enemy1Prefab);
                break;
        }
        if(go)
        {
            //#.오브젝트에 hpBar할당
            GameObject hpBar = Instantiate(hpBarPrefab);
            hpBar.GetComponent<HpSlider>().targetCharacter = go.GetComponent<Character>();
            hpBar.transform.SetParent(hpBarCanvas.transform);
            hpBar.transform.localScale = new Vector3(0.15f, 0.3f, 1);
            hpBar.transform.localPosition = new Vector3(1, 1, 1);
        }
        else
        {
            Debug.LogError("존재하지않는 오브젝트, BattleSystem");
        }
        return go;
    }
    //오브젝트 전달 
    public List<GameObject> GetEnemyTObjectList()
    {
        return enemyTList;
    }
    public List<GameObject> GetPlayerTObjectList()
    {
        List<GameObject> tempList = new List<GameObject>();
        foreach(CompanyBox tempCB in playerTList)
            tempList.Add(tempCB.character);
        return tempList;
    }
    //SetCost
    public void Plus_Cost(int plus_cost, CharacterSide characterSide)
    {
        if(characterSide == CharacterSide.Player)
            playerTCompositeCost += plus_cost;
        else
            enemyTCompositeCost += plus_cost;
    }
    public void Minus_Cost(int minus_cost, CharacterSide characterSide)
    {
        if (characterSide == CharacterSide.Player)
            playerTCompositeCost -= minus_cost;
        else
            enemyTCompositeCost -= minus_cost;
    }
    //#.Json 
    [ContextMenu("To Json Data")]
    void SaveCharacterDataToJson()
    {
        //#.스폰 정보 저장
        string jsonData = JsonUtility.ToJson(spawnData);
        string path = Application.dataPath + "/JsonFile/BattleData"; 
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("From Json Data")]
    public void load()
    {
        //#.스폰 정보 로드
        string path = Application.dataPath + "/JsonFile/BattleData"; 
        string jsonData = File.ReadAllText(path);
        spawnData = JsonUtility.FromJson<SpawnData>(jsonData);
    }
    //#.
    float DecreaseFunc(float x)
    {
        return -Mathf.Pow(x - 1, 2) + 1;
    }
}

