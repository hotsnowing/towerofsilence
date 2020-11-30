using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CostText : MonoBehaviour
{
    Text text;
    public BattleSystem battleSystem;
    private void Awake()
    {
        text = GetComponent<Text>();
    }
    private void LateUpdate()
    {
        text.text = "Cost:" + (battleSystem.playerTCompositeCost).ToString();
    }
}
