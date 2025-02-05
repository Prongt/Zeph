﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FloatVariable", menuName = "Variables/FloatVariable", order = 1)]
public class FloatVariable : ScriptableObject
{
//#if UNITY_EDITOR
//          [Multiline]
//          public string VariableDescription = "Optional";
//      #endif
    public float Value;

    public void SetValue(float value)
    {
        Value = value;
    }

    public void SetValue(FloatVariable value)
    {
        Value = value.Value;
    }

    public void ApplyChange(float amount)
    {
        Value += amount;
    }

    public void ApplyChange(FloatVariable amount)
    {
        Value += amount.Value;
    }
}

