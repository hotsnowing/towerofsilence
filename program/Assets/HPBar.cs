using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider hpSlider;

    public void SethpSlider(Character chara)
    {
        hpSlider.maxValue = chara.maxHp;
        hpSlider.value = chara.currentHp;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

}
