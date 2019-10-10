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
    [Tooltip("Only used by the player")][SerializeField] private float playerRange;
    [SerializeField] private Color debugColor;
    public List<AspectType> Promotes => promotes;

    public List<AspectType> Negates => negates;

    public Color DebugColor
    {
        get => debugColor;
        //set => debugColor = value;
    }

    public float PlayerRange
    {
        get => playerRange;
    }
}

/*
 * Light Cone
 * Wind Directional
 * Fire Touch
 */