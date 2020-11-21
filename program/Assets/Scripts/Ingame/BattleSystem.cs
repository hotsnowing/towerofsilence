using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

/*
 프르팹 버튼에 함수 부여하기  
 클릭으로 현재지정대상 정하기
 */

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST }
public enum PlayerTurnState {WaitForTurn, SelectCharacter, SelectSkillButton, SelectEnemy, CheckSkillOption}
//JsonFile**********************************
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
    //#.otherStatus

    //#.SkillData
    public SkillData[] cSkillData;
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
    [Header("SpawnData")]
    [SerializeField] private Vector3 playerFrontSpawnPos;
    [SerializeField] private Vector3 playerTSpawnDistance;
    [SerializeField] private Vector3 playerTHidePos;
    [SerializeField] private Vector3 enemyFrontSpawnPos;
    [SerializeField] private Vector3 enemyTSpawnDistance;
    [SerializeField] private Vector3 enemyTHidePos;
    [SerializeField] private float spawnDelayTime;
    private float pFixedMoveDistance;
    private float eFixedMoveDistance;

    private List<CompanyBox> playerTList;
    private List<GameObject> enemyTList;

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

    [Header("SkillManager")]
    public SkillManager skillManager;

    private GameObject playerObj;
    /*
     구현해야할 것
     클릭을 통한 공격할 유닛 선택 
     - 선택 -> 스킬패키지 순서 변경 
     */

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
    public bool isCharacterChanged;
    public bool isSkillChanged;
    private IEnumerator selectPlayerTCo;
    private IEnumerator selectSkillCo;
    private IEnumerator checkSkillOptionCo;
    private bool canChooseSkill = false;
    public CharacterType nowChoosen = CharacterType.Player;
    public CharacterType lastChoosen = CharacterType.Player;
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
        StartCoroutine(selectSkillCo = SelectSkill()); //첫번째는 플레이어 -> 스킬선택 실행
        ChooseCharacter.instance.isChoosing = true; //선택ON
        while (true)
        {
            //#.캐릭터 선택 변경
            if(nowChoosen != lastChoosen)
            {
                foreach(CompanyBox companyBox in playerTList)
                {
                    if(companyBox.character.GetComponent<Character>().characterType == nowChoosen)
                    {
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
            canChooseSkill = true;
            if (isCharacterChanged && playerTurnState == PlayerTurnState.SelectSkillButton) //캐릭터 변경됨
            {
                isCharacterChanged = false;
            }
            yield return null;
        }
    }
    IEnumerator CheckSkillOption(SkillPackage skillPackage, SkillButton skillButton)
    {
        playerTurnState = PlayerTurnState.CheckSkillOption;
        while (true)
        {
            SkillData skillData = skillButton.GetComponent<SkillButton>().skillData;
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
            if (skillManager.CheckSkillOption(skillData))
            {
                ischeckingSkillOption = false;
                playerTurnState = PlayerTurnState.SelectSkillButton;
                //#.조건충족 -> 버튼 재정렬
                skillPackage.SortButtons(skillButton);

                //구현 - 마나체크해서 더할지 턴넘길지 선택

                StopCoroutine(selectPlayerTCo);
                ChooseCharacter.instance.isChoosing = false;
                StopCoroutine(selectSkillCo);
                playerTurnState = PlayerTurnState.WaitForTurn;

                //임시구현
                StartCoroutine(PlayEnemyTurn());

                yield break;
            }
            yield return null;
        }
    }
    public void ActivateSkill(SkillPackage skillPackage, SkillButton skillButton)
    {
        if (battleState != BattleState.PLAYERTURN || !canChooseSkill) return;
        if (ischeckingSkillOption)
        {
            isSkillChanged = true;
            lastSkillButton.GetComponent<Animator>().SetTrigger("Pressed");
            StopCoroutine(checkSkillOptionCo);
            lastSkillButton = skillButton;
        }
        if (!lastSkillButton)
        {
            isCharacterChanged = false; //스킬버튼이 첫선택이라면 캐릭터 변경이 무의미하다.
            lastSkillButton = skillButton;
        } 

        //#.Check
        StartCoroutine(checkSkillOptionCo = CheckSkillOption(skillPackage, skillButton));
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
            SkillPackage skPk = skillPackage.GetComponent<SkillPackage>();

            //#.SpawnData로 부터 해당 인덱스의 스킬데이터를 받아옴 -> 스킬패키지 내용 구성
            SkillData[] skSt = spawnData.playerTSD[i].cSkillData;
            //#.박스에 오브젝트와 스킬페키지를 넣는다.
            CompanyBox companyBox = new CompanyBox(MakeObj(pT[i].cType), skillPackage);
            if(companyBox.character.tag == "Player")
            {
                playerObj = companyBox.character;
            }
            //#.스킬버튼에 랜덤으로 스킬부여
            List<int> list = MixOrder(skSt.Length);
            List<SkillData> transSkillData = new List<SkillData>();
            for(int j=0; j<list.Count; j++)
            {
                //#.랜덤값에 따라 스킬데이터 저장
                transSkillData.Add(skSt[j]);
            }
            //#.스킬데이터 전달.
            skPk.spawnSkillData = transSkillData;
            //#.스킬패키지 작동
            skPk.StartSkillPackage();

            //#.안보이는 위치로 이동
            companyBox.character.transform.position = playerTHidePos;

            //#.플레이어 리스트에 추가
            playerTList.Add(companyBox);
        }
        //#.Sort
        foreach (CompanyBox pTobj in playerTList)
        {
            //#.보이는 순서 지정 첫번째는 플레이어로 하고 다음부턴 선택으로
            if (pTobj.character.GetComponent<Character>().characterType == CharacterType.Player)
            {
                //#.플레이어에게 첫번째 위치부여
                pTobj.skillPackage.transform.SetAsLastSibling();
            }
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
            GameObject gameObject = MakeObj(pT[i].cType);
            gameObject.transform.position = enemyTHidePos;
            enemyTList.Add(gameObject);
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
            hpBar.transform.SetParent(hpBarCanvas.transform);
            hpBar.GetComponent<HPBar>().SethpSlider(go);
        }
        else
        {
            Debug.LogError("존재하지않는 오브젝트, BattleSystem");
        }
        return go;
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

    //#.Mix
    private List<int> MixOrder(int n)
    {
        int[] num = new int[n];
        List<int> list = new List<int>();
        for(int i=0; i<num.Length; i++)
        {
            num[i] = 0;
        }
        while (true)
        {
            int rand = Random.Range(0, num.Length);
            if(num[rand] == 0)
            {
                num[rand] = 1;
                list.Add(rand);
            }
            if (list.Count == num.Length) break;
        }
        return list;
    }
}

