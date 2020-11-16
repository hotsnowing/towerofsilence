using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider hpSlider;
    private GameObject targetObj;
    RectTransform rectTransform;
    private Vector3 sliderDistance;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SethpSlider(GameObject target)
    {
        targetObj = target;
        Character targetLogic= targetObj.GetComponent<Character>();
        hpSlider.maxValue = targetLogic.maxHp;
        hpSlider.value = targetLogic.currentHp;

        sliderDistance =new Vector3(0, target.GetComponent<Character>().height, 0);
        //#.사이즈 오류
        rectTransform.localScale = new Vector3(1, 1, 1);
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
    private void Update()
    {
        //#.항상 타겟과 함께 움직이게
         rectTransform.position = targetObj.transform.position + sliderDistance;
    }
}
