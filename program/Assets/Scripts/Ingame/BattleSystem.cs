using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    Character playerCharacter;
    Character enemyCharacter;

    public Text turnState;

    public HPBar playerHPBar;
    public HPBar enemyHPBar;
    public BattleState state;


    public SkillPakage skillPakage;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGo = Instantiate(playerPrefab);
        playerCharacter = playerGo.GetComponent<Character>();

        GameObject enemyGo =Instantiate(enemyPrefab);
        enemyCharacter = enemyGo.GetComponent<Character>();

        playerHPBar.SethpSlider(playerCharacter);
        enemyHPBar.SethpSlider(enemyCharacter);
        yield return new WaitForSeconds(2f);
        PlayerTurn();
    }

    IEnumerator PlayerSkill(int skillnumber)
    {
        //Character target = ChooseTarget();
        //playerCharacter.useSkill(target, skillnumber);

        bool isDead = enemyCharacter.TakeDamage(playerCharacter.atkPower);
        enemyHPBar.SetHP(enemyCharacter.currentHp);
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
        bool isDead = playerCharacter.TakeDamage(enemyCharacter.atkPower);
        playerHPBar.SetHP(playerCharacter.currentHp);
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
        if(state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerSkill(skillbutton.skillNumber));
        //#.스킬패키지 정렬
        skillPakage.SortButtons(skillbutton);
    }

}

