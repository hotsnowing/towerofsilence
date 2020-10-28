using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class SkillPakage : MonoBehaviour
{
    [Header("SkillButtons")]
    [SerializeField] private GameObject[] skillButtons;
    private RectTransform[] skillButtonsRT;
    private SkillButton[] skillButtonLogic;
    private int numOfSkillButton;
    [SerializeField] private Vector3 firstButtonLocalPos;
    [SerializeField] private float buttonDistance;
    [SerializeField] private Vector3[] buttonSpawnPos;
    [SerializeField] private List<int> buttonIndexArray;
    private bool isFirst = true;

    [Header("ButtonMove")]
    private IEnumerator[] moveButtonCoroutine;
    [SerializeField] private float buttonMoveDelayTime;
    private float moveDelayT; //버튼 이동시간 계수

    private void Awake()
    {
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

            //#.버튼 속성 랜덤 배정 - 미완
            skillButtonLogic[i].skillNumber = RandomSkill();

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
        //#.이동중 버튼을 클릭할 수 있음으로 이동중인 코루틴을 뭠춰준다.
        for(int i=0; i<numOfSkillButton; i++)
        {
            if (moveButtonCoroutine[i] != null)
            {
                StopCoroutine(moveButtonCoroutine[i]);
            }
        }

        //#.움직일 첫번쨰 인덱스
        int curIndex = 0;
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

            //#.누른버튼에 마지막 위치 인덱스값을 넣어줌
            skillButton.index = numOfSkillButton - 1;

            //#.마지막버튼 다시랜덤화
            skillButton.skillNumber = RandomSkill();
        }

        //#.다른버튼 부드러운 이동
        while (curIndex < numOfSkillButton)
        {
            //#.위치인덱스값 수정
            if (!isFirst)
            {
                //마지막 버튼은 누른 버튼으로 위에 설정했음으로 반복문을 종료한다.
                if (curIndex == numOfSkillButton - 1) break;
                skillButtonLogic[buttonIndexArray[curIndex]].index--;
            }
            else
            {
                if (curIndex == numOfSkillButton - 1)
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
    private int RandomSkill()
    {
        return 1;
    }

}
