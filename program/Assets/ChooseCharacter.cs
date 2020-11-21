using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChooseCharacter : MonoBehaviour
{
    public BattleSystem battleSystem;
    public GameObject MousePointer;
    public GameObject testMarkP;
    public GameObject testMarkE;
    public bool isChoosing;

    [Header("Choosen")]
    public GameObject playerT0;
    public GameObject enemyT0;

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
        {
            Destroy(gameObject);
        }
    }
    #endregion singleton


    private void Update()
    {
        //마우스 포인터
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePointer.transform.position = new Vector3(p.x, p.y, -5);

        if (isChoosing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hitP = Physics2D.Raycast(p, Vector3.back, 100, LayerMask.GetMask("PlayerTCollider"));
                RaycastHit2D hitE = Physics2D.Raycast(p, Vector3.back, 100, LayerMask.GetMask("EnemyTCollider"));
                if (hitP.collider != null)
                { 
                    testMarkP.SetActive(true);
                    playerT0 = hitP.transform.gameObject;
                    testMarkP.transform.position = hitP.transform.position + new Vector3(0, 2, 0);
                    //#.선택된 캐릭터 타입
                    battleSystem.nowChoosen = hitP.transform.parent.GetComponent<Character>().characterType;
                }
                else if (hitE.collider != null)
                {
                    testMarkE.SetActive(true);
                    enemyT0 = hitE.transform.gameObject;
                    testMarkE.transform.position = hitE.transform.position + new Vector3(0, 2, 0);
                }
                else
                {
                    testMarkE.SetActive(false);
                }
            }
        }
        else
        {
            testMarkP.SetActive(false);
            testMarkE.SetActive(false);
            playerT0 = null;
            enemyT0 = null;
        }
    }
}
