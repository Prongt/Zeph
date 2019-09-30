using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Aspects/Element", order = 2)]
public class Element : ScriptableObject
{
    public string ElementType;
    public List<AspectTypes> affectsTheseAspects;
}

//[Serializable]
//public enum Elements
//{
//    light, 
//    wind,
//    heat
//}

/*
 * Light Cone
 * Wind Directional
 * Fire Touch
 */
