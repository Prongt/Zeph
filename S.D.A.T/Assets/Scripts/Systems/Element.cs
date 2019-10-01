using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Aspects/Element", order = 2)]
public class Element : ScriptableObject
{
    //[SerializeField] private ElementsEnum elementEnum;
    [SerializeField] private List<AspectType> promotes;
    [SerializeField] private List<AspectType> negates;

    public List<AspectType> Promotes => promotes;

    public List<AspectType> Negates => negates;
}

/*
 * Light Cone
 * Wind Directional
 * Fire Touch
 */