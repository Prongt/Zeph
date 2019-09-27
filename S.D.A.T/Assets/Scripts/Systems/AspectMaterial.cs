using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AspectMaterial", menuName = "Aspects/Material", order = 1)]
public class AspectMaterial : ScriptableObject
{
    public AspectTypes[] AspectTypes;
}


//Damagable
/*
 * damage effect
 * damaged material
 *
 * damage resistance
 */


//Fire
/*
 * burning material
 * burning particle effect
 * burned material
 *
 * burn rate (10% health per second)
 */

