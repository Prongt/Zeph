using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Sets the eventSystems selected object to null when called
/// </summary>
public class SelectorReset : MonoBehaviour
{
    public EventSystem eventSystem;
    
    public void ResetSelectedButton()
    {
        eventSystem.SetSelectedGameObject(null);
    } 
}
