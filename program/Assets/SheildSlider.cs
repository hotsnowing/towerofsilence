using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheildSlider : MonoBehaviour
{
    [Header("BasicData")]
    public Character targetCharacter;
    public float maxValue;
    public float curValue;
    public float lastValue;
    public Image mainBar;
    public Image backEffect;
    public float fastT;
    public float slowT;
    public Vector3 distance;

    [Header("BackColor")]
    public Color damageColor;
    public Color healColor;

    [Header("MainColor")]
    public Color color;

    private float mt;
    private float dt;

    private void LateUpdate()
    {
        if(targetCharacter != null)
        {
            curValue = targetCharacter.curShield;
            maxValue = targetCharacter.maxShield;
            mainBar.color = color;

            float t = 1 / maxValue;
            if (mainBar.fillAmount <= curValue * t)
            {
                //회복
                mt = slowT;
                dt = fastT;
                backEffect.color = healColor;
            }
            else
            {
                //피해
                mt = fastT;
                dt = slowT;
                backEffect.color = damageColor;
            }
            mainBar.fillAmount = Mathf.Lerp(mainBar.fillAmount, curValue * t, mt);
            backEffect.fillAmount = Mathf.Lerp(backEffect.fillAmount, curValue * t, dt);
        }
    }
}
