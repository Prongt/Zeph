using UnityEngine;

public class PowerCollectionPoint : MonoBehaviour
{
    [SerializeField] private ElementState elementState;
    private void OnTriggerEnter(Collider col)
    {
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
    }
}
