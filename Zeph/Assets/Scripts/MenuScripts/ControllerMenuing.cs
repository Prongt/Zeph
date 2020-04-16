using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class ControllerMenuing : MonoBehaviour
{
    public EventSystem eve;
    public GameObject defaultButton;

    private void OnEnable()
    {
        eve.SetSelectedGameObject(defaultButton);
    }

    void Update()
    {
        //Resets the default button if mouse is used and then controller is used
        if (eve.currentSelectedGameObject == null)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                eve.SetSelectedGameObject(defaultButton);
            }
        }    
    }

    private void OnDisable()
    {
        eve.SetSelectedGameObject(null);
    }
}
