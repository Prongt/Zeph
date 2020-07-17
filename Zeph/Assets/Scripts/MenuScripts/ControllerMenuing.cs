using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Enables the user interface to be compatible with gamepad controllers
/// </summary>
public class ControllerMenuing : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject defaultButton;

    private void OnEnable()
    {
        eventSystem.SetSelectedGameObject(defaultButton);
    }

    void Update()
    {
        //Resets the default button if mouse is used and then controller is used
        if (eventSystem.currentSelectedGameObject != null) return;
        
        if (Math.Abs(Input.GetAxis("Vertical")) > 0.01f || Math.Abs(Input.GetAxis("Horizontal")) > 0.01f)
        {
            eventSystem.SetSelectedGameObject(defaultButton);
        }
    }

    private void OnDisable()
    {
        eventSystem.SetSelectedGameObject(null);
    }
}
