using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
public class SkillPackage : MonoBehaviour
{
    [Header("SpawnSkillList")]
    public List<SkillDataBox> spawnSkill_DataBox; //생성할 스킬버튼의 종류및 수 리스트
    private int skillNumberListIndex;

    [Header("SkillButtons")]
    [SerializeField] private GameObject[] skillButtons; //표시 버튼 + 1
    private RectTransform[] skillButtonsRT;
    [System.NonSerialized]public SkillButton[] skillButtonLogic;
    private int numOfSkillButton;
    private int numOfActiveSkillButton;
    [SerializeField] private Vector3 firstButtonLocalPos;
    [SerializeField] private float buttonDistance;
    private Vector3[] buttonSpawnPos;
    private List<int> buttonIndexArray;
    private bool isFirst = true;

    [Header("ButtonMove")]
    [SerializeField] private float buttonMoveDelayTime;
    private float moveDelayT; //버튼 이동시간 계수

    [Header("BattleSystem")]
    public BattleSystem battleSystem;

    [Header("CheckCost")]
    public int minCost = 100;

    private void Awake()
    {
        spawnSkill_DataBox = new List<SkillDataBox>();

        numOfSkillButton = skillButtons.Length;
    }
    private void Update()
    {
        //#.버튼 활성화 비활성화
        if(battleSystem.playerTurnState == PlayerTurnState.SelectSkillButton || battleSystem.playerTurnState == PlayerTurnState.CheckSkillOption)
        {
            //#.선택가능한것만 Enabled로 바꾼다.
            foreach(GameObject tempSkill_Button in skillButtons)
            {
                int temp_Id = tempSkill_Button.GetComponent<SkillButton>().skillDataBox.skill_Id;
                int tempCost = SkillManager.instance.GetBasicSkillData(temp_Id).skill_Cost;
                if (battleSystem.playerTCompositeCost >= tempCost && tempSkill_Button.activeSelf)
                {
                    tempSkill_Button.GetComponent<Button>().interactable = true;
                    //#.사용가능 스킬중 가장 코스트가 낮은 것을 찾는다.
                    if (minCost >= tempCost)
                        minCost = tempCost;
                }
                else
                {
                    //.
                }

            }     
        }
        else
        {
            foreach (GameObject tempSkill_Button in skillButtons)
                tempSkill_Button.GetComponent<Button>().interactable = false;
        }
    }
    public void StartSkillPackage()
    {
        skillNumberListIndex = 0; //리스트를 앞에서부터 접근

        //버튼 이동시간 계수
        moveDelayT = 1 / buttonMoveDelayTime;

        skillButtonsRT = new RectTransform[numOfSkillButton];
        skillButtonLogic = new SkillButton[numOfSkillButton];

        buttonIndexArray = new List<int>();

        buttonSpawnPos = new Vector3[numOfSkillButton];
        for (int i=0; i< numOfSkillButton; i++)
        {
            //#.버튼을 일단 꺼둔다
            skillButtons[i].SetActive(false);
            //#.버튼 인덱스를 넣어준다.
            buttonIndexArray.Add(i);

            //#.버튼들의 RectTransForm
            skillButtonsRT[i] = skillButtons[i].GetComponent<RectTransform>();

            //#.SkillButtonScript
            skillButtonLogic[i] = skillButtons[i].GetComponent<SkillButton>();

            if(i < spawnSkill_DataBox.Count)
            {
                //#.버튼넘버(속성) 부여
                skillButtonLogic[i].skillDataBox = GiveSkill_DataBox();
                skillButtonLogic[i].SetText();
            }

            //#.버튼 스폰 포인트 지정
            buttonSpawnPos[i] = firstButtonLocalPos + new Vector3(buttonDistance, 0, 0) * i;
        }
        //#.Start
        SortButtons();
    }
    //#.PushButton
    public void PushButton(SkillButton skillButton)
    {
        battleSystem.ActivateSkill(this, skillButton);
    }
    //#.스킬버튼 정렬 case1) 처음 정렬 case2)선택시 버튼사라진후 정렬
    public void SortButtons(SkillButton skillButton = null)
    {
        //#.정렬
        StartCoroutine(SortSkillButtonsCoroutine(skillButton));
    }
    private IEnumerator SortSkillButtonsCoroutine(SkillButton skillButton = null)  //클릭한 버튼
    {
        int curIndex = 0;  // 움직일 인덱스
        IEnumerator[] moveCoroutine = new IEnumerator[numOfSkillButton];
        if (!isFirst)
        {
            int removeIndex = skillButton.index;

            //#.해당버튼의 위치의 값을 제거하고 마지막에 넣어준다.
            buttonIndexArray.Add(buttonIndexArray[removeIndex]);
            buttonIndexArray.RemoveAt(removeIndex);

            //#.시작인덱스
            curIndex = removeIndex;

            //#.누른버튼 설정
            //#.가장뒤로 이동
            skillButtonsRT[buttonIndexArray[numOfSkillButton - 1]].anchoredPosition = buttonSpawnPos[numOfSkillButton - 1];
            skillButtons[buttonIndexArray[numOfSkillButton - 1]].GetComponent<Button>().interactable = false;
            skillButtons[buttonIndexArray[numOfSkillButton - 1]].SetActive(false);
            if (skillNumberListIndex == spawnSkill_DataBox.Count) //모든 버튼이 생성됨
            {
                numOfActiveSkillButton--; //누른 버튼을 버림
            }
            else  //생성될 버튼이 아직남음
            {
                //#.누른버튼에 마지막 위치 인덱스값을 넣어줌
                skillButton.index = numOfSkillButton - 1;

                //#.버튼넘버(속성) 부여
                skillButton.skillDataBox = GiveSkill_DataBox();
            }
        }
        else //첫 스폰일 떄
        {
            numOfActiveSkillButton = spawnSkill_DataBox.Count >= numOfSkillButton ? 
                numOfSkillButton : spawnSkill_DataBox.Count;
        }
        //#.누르지않은 버튼 부드러운 이동
        while (curIndex < numOfActiveSkillButton)
        {
            //#.위치인덱스값 수정
            if (!isFirst)
            {
                //마지막 버튼은 누른 버튼으로 위에 설정했음으로 반복문을 종료한다.
                if (curIndex == skillButtons.Length - 1) break;
                skillButtonLogic[buttonIndexArray[curIndex]].index--;
            }
            else
            {
                if (curIndex == skillButtons.Length - 1)
                {
                    //#.영역이탈 시 마지막 버튼 비활성화
                    skillButtons[buttonIndexArray[curIndex]].SetActive(false);
                    break;
                }
            }
            yield return StartCoroutine(moveCoroutine[curIndex] = MoveButton(curIndex));
            curIndex++;
        }

        //#.첫배치 종료
        if (isFirst)
        {
            isFirst = false;
        }
        yield break;
    }
    private IEnumerator MoveButton(int index)
    {
        float t = 0;
        int moveIndex = index;
        //#.SetActiveButton
        if(!skillButtons[buttonIndexArray[index]].activeSelf)
            skillButtons[buttonIndexArray[index]].SetActive(true);

        Vector3 orgPos = skillButtonsRT[buttonIndexArray[index]].anchoredPosition;
        while (t < 1)
        {
            t += Time.deltaTime * moveDelayT;
            //#.이동
            skillButtonsRT[buttonIndexArray[index]].anchoredPosition = Vector3.Lerp(orgPos, buttonSpawnPos[moveIndex], t);
            yield return null;
        }
        yield break;
    }

    //#.미완
    private SkillDataBox GiveSkill_DataBox()
    {
        return spawnSkill_DataBox[skillNumberListIndex++];
    }
}
