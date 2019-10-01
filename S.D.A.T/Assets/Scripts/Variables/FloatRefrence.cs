using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FloatReference
{
   [SerializeField] private bool UseConstant = true;
   [SerializeField] private float ConstantValue;
   [SerializeField] private FloatVariable Variable;

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
