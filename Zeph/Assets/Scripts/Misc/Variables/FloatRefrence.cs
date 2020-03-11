using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FloatReference
{
   [SerializeField] private bool UseConstant = true;
   //[HideIf("UseConstant", false)][SerializeField] private float ConstantValue;
   [SerializeField] private float ConstantValue;
   //[HideIf("UseConstant", false, true)][SerializeField] private FloatVariable Variable;
   [SerializeField] private FloatVariable Variable = default;

    public FloatReference()
    { }

    public FloatReference(float value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public float Value
    {
        //Returns a manually set value or a float variable
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator float(FloatReference reference)
    {
        return reference.Value;
    }
}
