using System;
using UnityEngine;

public class PowerDisabler : MonoBehaviour
{
    [SerializeField] private ElementState[] elementsToDisable;
    private PlayerElementController playerElementController;
    private void Start()
    {
        playerElementController = FindObjectOfType<PlayerElementController>();

        ElementStateChanger();
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
