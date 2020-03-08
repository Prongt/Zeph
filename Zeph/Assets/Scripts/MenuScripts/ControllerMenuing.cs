﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class ControllerMenuing : MonoBehaviour
{
    [SerializeField] private List<Button> menuOptions;
    private GameObject selector;
    
    //This may be redundant
    [Header("Number of Menu Options")]
    [Tooltip("Never make this more than the number of buttons in the menu")]
    [Range(0, 10)] public int curPos;
    
    void Start()
    {
        //Gameobject that tracks the current position of the selector
        selector = new GameObject("Selector");
        //Sets the starting Selector position
        if (menuOptions != null)
        {
            selector.transform.position = menuOptions[curPos].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Moves the selector up and down in the menu
        if (Input.GetAxis("Vertical") < 0 && curPos < menuOptions.Count)
        {
            curPos += 1;
            selector.transform.position = menuOptions[curPos].transform.position;
        } else if (Input.GetAxis("Vertical") > 0 && curPos > 0)
        {
            curPos -= 1;
            selector.transform.position = menuOptions[curPos].transform.position;
        }

        if (selector.transform.position == menuOptions[curPos].transform.position)
        {
            menuOptions[curPos].Select();
        }
    }
}