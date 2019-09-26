using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ElementalMaterial", menuName = "Elemental/Material", order = 1)]
public class ElementalMaterial : ScriptableObject
{
    public MaterialProperties[] MaterialProperties;
}

[Serializable]
public struct MaterialProperties
{
    public AspectTypes AspectType;
    
    //[Range(0f,1f)]public float Ratio;
}

