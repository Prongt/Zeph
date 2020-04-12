using System;
using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapControl : MonoBehaviour
{
    public EventSystem eve;
    public GameObject defaultButton;
    private bool isOpen;

    private void OnEnable()
    {
        isOpen = true;
    }

    void Update()
    { 
        //freezes the player
        PlayerMoveRigidbody.HaltMovement = true;
        if (isOpen)
        {
            eve.SetSelectedGameObject(defaultButton);
            isOpen = false;
        }
        
        //Resets the button if mouse is used and then the controller is used
        if (eve.currentSelectedGameObject == null)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                eve.SetSelectedGameObject(defaultButton);
            }
        }
    }
}
