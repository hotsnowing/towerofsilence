﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public int index; //현재 위치index
    public Text text;
    public SkillData skillData;

    //#.Test
    public void SetText()   
    {
        text.text = (skillData.skillUser).ToString();
    }
}
