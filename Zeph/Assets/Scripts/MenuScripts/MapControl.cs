using System;
using Movement;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Level selection controller
/// </summary>
public class MapControl : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject defaultButton;
    private bool isOpen;

    private void OnEnable()
    {
        isOpen = true;
    }

    private void Update()
    { 
        //freezes the player
        PlayerMoveRigidbody.HaltMovement = true;
        if (isOpen)
        {
            eventSystem.SetSelectedGameObject(defaultButton);
            isOpen = false;
        }
        
        //Resets the button if mouse is used and then the controller is used
        if (eventSystem.currentSelectedGameObject == null)
        {
            if (Math.Abs(Input.GetAxis("Vertical")) > 0.01f || Math.Abs(Input.GetAxis("Horizontal")) > 0.01f)
            {
                eventSystem.SetSelectedGameObject(defaultButton);
            }
        }
    }
}
