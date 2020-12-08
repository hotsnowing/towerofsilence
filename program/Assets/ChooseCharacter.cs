using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChooseCharacter : MonoBehaviour
{
    public BattleSystem battleSystem;
    public GameObject MousePointer;
    public GameObject testMarkP;
    public GameObject testMarkE;
    public bool canChoosePlayerT;
    public bool canChooseEnemyT;

    [Header("Choosen")]
    public GameObject skill_User_Collider;
    public GameObject skill_Target_Collider;

    static public ChooseCharacter instance;
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
    }
    #endregion singleton
    private void Update()
    {
        //마우스 포인터
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePointer.transform.position = new Vector3(p.x, p.y, -5);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hitP = Physics2D.Raycast(p, Vector3.back, 100, LayerMask.GetMask("PlayerTCollider"));
            RaycastHit2D hitE = Physics2D.Raycast(p, Vector3.back, 100, LayerMask.GetMask("EnemyTCollider"));
            if (canChoosePlayerT)
            {
                if (hitP.collider != null)
                {
                    testMarkP.SetActive(true);
                    skill_User_Collider = hitP.transform.gameObject;
                    testMarkP.transform.position = hitP.transform.position + new Vector3(0, 2, 0);
                    //#.선택된 캐릭터 타입
                    battleSystem.nowChoosen = hitP.transform.parent.GetComponent<Character>().characterType;
                }
            }

            if (canChooseEnemyT)
            {
                if (hitE.collider != null)
                {
                    testMarkE.SetActive(true);
                    skill_Target_Collider = hitE.transform.gameObject;
                    testMarkE.transform.position = hitE.transform.position + new Vector3(0, 2, 0);
                }
            }

            if(!canChooseEnemyT && !canChoosePlayerT)
            {
                testMarkP.SetActive(false);
                testMarkE.SetActive(false);
                skill_User_Collider = null;
                skill_Target_Collider = null;
            }
        }
    }
    public GameObject GetSkill_User()
    {
        if (skill_User_Collider) return skill_User_Collider.transform.parent.gameObject;
        return null;
    }
    public GameObject GetSkill_Target()
    {
        if (skill_Target_Collider) return skill_Target_Collider.transform.parent.gameObject;
        return null;
    }
    public void ResetAllTarget()
    {
        battleSystem.nowChoosen = CharacterType.None;
        battleSystem.lastChoosen = CharacterType.None;
        testMarkP.SetActive(false);
        testMarkE.SetActive(false);
        skill_User_Collider = null;
        skill_Target_Collider = null;
    }
    public void Set_Target_And_User(GameObject user = null, GameObject target = null)
    {
        skill_User_Collider = user;
        skill_Target_Collider = target;
    }
    public void ResetEnemyTarget()
    {
        testMarkE.SetActive(false);
        skill_Target_Collider = null;
    }

}
