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
    [SerializeField] private string buttonName;
    [SerializeField] private bool powerIsEnabled;
    [HideInInspector] public Collider[] colliders;
    public List<AspectType> Promotes => promotes;
    public List<AspectType> Negates => negates;
    public List<string> promoteStrings;
    public List<string> negateStrings;

    private void OnValidate()
    {
        promoteStrings.Clear();
        foreach (AspectType aspectType in promotes)
        {
            promoteStrings.Add(aspectType.ToString());
        }
        
        negateStrings.Clear();
        foreach (AspectType aspectType in negates)
        {
            negateStrings.Add(aspectType.ToString());
        }
    }

    

    public bool PowerIsEnabled
    {
        get => powerIsEnabled;
        set => powerIsEnabled = value;
    }

    public Color DebugColor
    {
        get => debugColor;
        //set => debugColor = value;
    }

    public float PlayerRange
    {
        get => playerRange;
    }

    public string ButtonName
    {
        get => buttonName;
    }
}

/*
 * Light Cone
 * Wind Directional
 * Fire Touch
 */