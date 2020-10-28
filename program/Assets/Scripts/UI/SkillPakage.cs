using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SkillPakage : MonoBehaviour
{
    [Header("SpawnSkillList")]
    public List<int> spawnSkillNumber; //생성할 스킬버튼의 종류및 수 리스트
    private int skillNumberListIndex;

    [Header("SkillButtons")]
    [SerializeField] private GameObject[] skillButtons; //표시 버튼 + 1
    private RectTransform[] skillButtonsRT;
    private SkillButton[] skillButtonLogic;
    private int numOfSkillButton;
    private int numOfActiveSkillButton;
    [SerializeField] private Vector3 firstButtonLocalPos;
    [SerializeField] private float buttonDistance;
    private Vector3[] buttonSpawnPos;
    private List<int> buttonIndexArray;
    private bool isFirst = true;

    [Header("ButtonMove")]
    private IEnumerator[] moveButtonCoroutine;
    [SerializeField] private float buttonMoveDelayTime;
    private float moveDelayT; //버튼 이동시간 계수

    private void Awake()
    {
        skillNumberListIndex = 0; //리스트를 앞에서부터 접근

        //버튼 이동시간 계수
        moveDelayT = 1 / buttonMoveDelayTime;

        numOfSkillButton = skillButtons.Length;

        skillButtonsRT = new RectTransform[numOfSkillButton];
        skillButtonLogic = new SkillButton[numOfSkillButton];

        buttonIndexArray = new List<int>();

        buttonSpawnPos = new Vector3[numOfSkillButton];
        for (int i=0;i<skillButtons.Length; i++)
        {
            //#.버튼을 일단 꺼둔다
            skillButtons[i].SetActive(false);
            //#.버튼 인덱스를 넣어준다.
            buttonIndexArray.Add(i);

            //#.버튼들의 RectTransForm
            skillButtonsRT[i] = skillButtons[i].GetComponent<RectTransform>();

            //#.SkillButtonScript
            skillButtonLogic[i] = skillButtons[i].GetComponent<SkillButton>();

            if(i < spawnSkillNumber.Count)
            {
                //#.버튼넘버(속성) 부여
                skillButtonLogic[i].skillNumber = GiveSkillNumber();
            }

            //#.버튼 스폰 포인트 지정
            buttonSpawnPos[i] = firstButtonLocalPos + new Vector3(buttonDistance, 0, 0) * i;
        }

        //#.코루틴 저장
        moveButtonCoroutine = new IEnumerator[numOfSkillButton];

        //#.Start
        SortButtons();
    }

    public void SortButtons(SkillButton skillButton = null)
    {
        StartCoroutine(SortSkillButtonsCoroutine(skillButton));
    }
    private IEnumerator SortSkillButtonsCoroutine(SkillButton skillButton = null)  //클릭한 버튼
    {
        int curIndex = 0;  // 움직일 인덱스

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
            skillButtons[buttonIndexArray[numOfSkillButton - 1]].SetActive(false);

            if (skillNumberListIndex == spawnSkillNumber.Count) //모든 버튼이 생성됨
            {
                numOfActiveSkillButton--; //누른 버튼을 버림
            }
            else  //생성될 버튼이 아직남음
            {
                //#.누른버튼에 마지막 위치 인덱스값을 넣어줌
                skillButton.index = numOfSkillButton - 1;

                //#.버튼넘버(속성) 부여
                skillButton.skillNumber = GiveSkillNumber();
            }
        }
        else //첫 스폰일 떄
        {
            numOfActiveSkillButton = spawnSkillNumber.Count >= numOfSkillButton ? numOfSkillButton : spawnSkillNumber.Count;
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
                    //#.첫배치시 마지막 버튼 비활성화
                    skillButtons[buttonIndexArray[curIndex]].SetActive(false);
                    break;
                }
            }
            yield return StartCoroutine(moveButtonCoroutine[curIndex] = MoveButton(curIndex));
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
    private int GiveSkillNumber()
    {
        return spawnSkillNumber[skillNumberListIndex++];
    }
}
