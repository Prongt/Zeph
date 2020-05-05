using System.Collections.Generic;
using Movement;
using UnityEngine;
using UnityEngine.UI;

public class UiFadeOut : MonoBehaviour
{
    [TextArea][SerializeField] private List<string> storyText = default;
    [SerializeField] private string buttonToClose = "Story";

    private Text text;
    private int storyIndex;
    
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        text.text = string.Empty;
        storyIndex = 0;
    }

    private void OnEnable()
    {
        text.text = string.Empty;
    }

    private void Update()
    {
        DisplayText();
    }

    private void DisplayText()
    {
        PlayerMoveRigidbody.HaltMovement = true;
        text.text = storyText[storyIndex];
        if (Input.GetButtonDown(buttonToClose))
        {
            if (storyIndex == storyText.Count -1)
            {
                CloseUi();
                return;
            }
            storyIndex++;
        }
    }


    private void CloseUi()
    {
        gameObject.SetActive(false);
        storyIndex = 0;
        PlayerMoveRigidbody.HaltMovement = false;
    }
}
