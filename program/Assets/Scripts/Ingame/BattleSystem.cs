using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST }
[System.Serializable]
public class SpawnData
{
    public List<string> playerTList;
    public List<string> enemyTList;
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

    private List<GameObject> playerTList;
    private List<GameObject> enemyTList;

    [Header("MoveData")]
    [SerializeField] private float objMoveTime;
    [SerializeField] private SpawnData spawnData; //LoadFormJson **************

    [Header("Prefab")]
    //#.PlayerT
    public GameObject playerPrefab;
    public GameObject company0Prefab;
    public GameObject company1Prefab;
    //#.EnemyT
    public GameObject enemy0Prefab;
    public GameObject enemy1Prefab;

    public GameObject hpBarPrefab;
    //#.
    Character playerLogic;
    Character company0Logic;
    Character company1Logic;
    Character enemy0Logic;
    Character enemy1Logic;


    [Header("State")]
    public Text turnState;
    public BattleState state;

    [Header("Canvas")]
    public GameObject canvas;
    [SerializeField]private SkillPakage skillPakage;

    private void Awake()
    {
        playerTList = new List<GameObject>();
        enemyTList = new List<GameObject>();

        pFixedMoveDistance = Mathf.Abs(playerFrontSpawnPos.x - playerTHidePos.x);
        eFixedMoveDistance = Mathf.Abs(enemyTHidePos.x - enemyFrontSpawnPos.x); 
    }
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    { 
        load(); //Load JsonDataFile
        StartCoroutine(SpawnPlayerT());
        StartCoroutine(SpawnEnemyT());
        yield return new WaitForSeconds(2f);
        PlayerTurn();
    }
    IEnumerator PlayerSkill(int skillnumber)
    {
        //Character target = ChooseTarget();
        //playerCharacter.useSkill(target, skillnumber);

        bool isDead = enemy0Logic.TakeDamage(playerLogic.atkPower);

        //enemyHPBar.SetHP(enemy0Logic.currentHp);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator EnemyTurn()
    {
        turnState.text = "Enemy Turn";
        bool isDead = playerLogic.TakeDamage(enemy0Logic.atkPower);

        //playerHPBar.SetHP(playerLogic.currentHp);

        yield return new WaitForSeconds(1f);
        if (isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            PlayerTurn();
        }
        
    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            turnState.text = "YOU WIN";
        }
        else
        {

        }
        yield return new WaitForSeconds(2f);

        ++GameDataManager.Instance.CurrentStage;
        
        SceneManager.LoadScene("Map");
    }


    void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        turnState.text = "Player Turn";
    }

    //Character ChooseTarget()
    //{
      
    //}

    public void OnSkillButton(SkillButton skillbutton)
    {
        if(state != BattleState.PLAYERTURN) return;
        StartCoroutine(PlayerSkill(skillbutton.skillNumber));
        skillPakage.SortButtons(skillbutton); //SortSkillPakage 
    }
    //#.SpawnSystem
    IEnumerator SpawnPlayerT()
    { 
        Vector3 nowSpawnPos = playerFrontSpawnPos;
        List<string> pT = spawnData.playerTList;
        //#.Spawn
        for(int i=0; i<pT.Count; i++)
        {
            GameObject gameObject = MakeObj(pT[i]);
            gameObject.transform.position = playerTHidePos;
            playerTList.Add(gameObject);
        }
        //#.Sort
        foreach(GameObject pTobj in playerTList)
        {
            StartCoroutine(MoveObject(pTobj, nowSpawnPos, pFixedMoveDistance));
            nowSpawnPos += playerTSpawnDistance; 
            yield return new WaitForSeconds(spawnDelayTime); //SpawnDelay
        }
    }
    IEnumerator SpawnEnemyT()
    {
        Vector3 nowSpawnPos = enemyFrontSpawnPos;
        List<string> pT = spawnData.enemyTList;
        //#.Spawn
        for(int i=0; i<pT.Count; i++)
        {
            GameObject gameObject = MakeObj(pT[i]);
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
    private GameObject MakeObj(string objName)
    {
        GameObject go = null;
        switch (objName)
        {
            case "Player":
                go = Instantiate(playerPrefab);
                break;
            case "Company0":
                go =Instantiate(company0Prefab);
                break;
            case "Company1":
                go =Instantiate(company1Prefab);
                break;
            case "Enemy0":
                go =Instantiate(enemy0Prefab);
                break;
            case "Enemy1":
                go =Instantiate(enemy1Prefab);
                break;
        }
        if(go)
        {
            //#.오브젝트에 hpBar할당
            GameObject hpBar = Instantiate(hpBarPrefab);
            hpBar.transform.SetParent(canvas.transform);
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
}

