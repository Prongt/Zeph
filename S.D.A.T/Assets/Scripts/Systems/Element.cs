using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Aspects/Element", order = 2)]
public class Element : ScriptableObject
{
    [SerializeField] private ElementsEnum elementEnum;
    [SerializeField] private List<AspectType> promotes;
    [SerializeField] private List<AspectType> negates;

    public List<AspectType> Promotes
    {
        get { return promotes; }
    }
    
    public List<AspectType> Negates
    {
        get { return negates; }
    }
}

[Serializable]
public enum ElementsEnum
{
    light, 
    wind,
    heat
}

/*
 * Light Cone
 * Wind Directional
 * Fire Touch
 */
