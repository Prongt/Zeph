using UnityEngine;

public class PowerCollectionPoint : MonoBehaviour
{
    [SerializeField] private ElementState elementState;
    [SerializeField] private GameObject uiToActivate;
    private bool hasBeenActivated = false;
    private void OnTriggerEnter(Collider col)
    {
        if (hasBeenActivated) return;
        if (!col.CompareTag("Player")) return;

        var elementController = col.GetComponent<PlayerElementController>();
        if (!elementController) return;
        
        foreach (var elementData in elementController.elementData)
        {
            if (elementData.element == elementState.element)
            {
                elementData.element.PowerIsEnabled = elementState.isEnabled;
            }
        }
        
        if (uiToActivate) uiToActivate.SetActive(true);
        
        hasBeenActivated = true;
    }
}
