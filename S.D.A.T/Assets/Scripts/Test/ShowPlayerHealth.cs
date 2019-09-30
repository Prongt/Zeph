using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowPlayerHealth : MonoBehaviour
{
    
    [SerializeField] private FloatVariable playerHealth;
    [SerializeField] private Text UiText;
    [Multiline][SerializeField] private string text;


    private void Start()
    {
        if (GetComponent<Text>())
        {
            UiText = GetComponent<Text>();
        }
    }

    private void Update()
    {
        UiText.text = text + playerHealth.Value.ToString();
    }
}
