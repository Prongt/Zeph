﻿using System;
using UnityEngine;

/// <summary>
/// Disables all player abilities when scene starts
/// </summary>
public class PowerDisabler : MonoBehaviour
{
    [SerializeField] private ElementState[] elementsToDisable = default;
    private PlayerElementController playerElementController;
    private void Start()
    {
        playerElementController = FindObjectOfType<PlayerElementController>();

        ElementStateChanger();
    }

    private void OnDestroy()
    {
    //TODO remove duplicate code
        foreach (var elementData in playerElementController.elementData)
        {
            foreach (var elementState in elementsToDisable)
            {
                if (elementData.element == elementState.element)
                {
                    elementData.element.PowerIsEnabled = true;
                }
            }
        }
    }


    private void ElementStateChanger()
    {
        foreach (var elementData in playerElementController.elementData)
        {
            foreach (var elementState in elementsToDisable)
            {
                if (elementData.element == elementState.element)
                {
                    elementData.element.PowerIsEnabled = elementState.isEnabled;
                }
            }
        }
    }
}

[Serializable]
public struct ElementState
{
    public Element element;
    public bool isEnabled;
}
