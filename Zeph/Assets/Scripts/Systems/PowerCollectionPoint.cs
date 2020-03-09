
using UnityEngine;

public class PowerCollectionPoint : MonoBehaviour
{
    [SerializeField] private ElementState elementState;
    [SerializeField] private GameObject uiToActivate;
    private bool hasBeenActivated;
    private PlayerElementController playerElementController;
    private void Start()
    {
        playerElementController = FindObjectOfType<PlayerElementController>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (hasBeenActivated) return;
        if (!col.CompareTag("Player")) return;

        for (int i = 0; i < playerElementController.elementData.Length; i++)
        {
            var element = playerElementController.elementData[i].element;
            if (element == elementState.element)
            {
                element.PowerIsEnabled = elementState.isEnabled;
            }
        }

        // var elementController = col.GetComponent<PlayerElementController>();
        // if (!elementController) return;
        //
        // foreach (var elementData in elementController.elementData)
        // {
        //     if (elementData.element == elementState.element)
        //     {
        //         elementData.element.PowerIsEnabled = elementState.isEnabled;
        //     }
        // }
        
        if (uiToActivate) uiToActivate.SetActive(true);
        
        hasBeenActivated = true;
    }
}
