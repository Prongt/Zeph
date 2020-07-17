using System.Collections.Generic;
using Movement;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Iterates through the story dialog and then fades it out when finished
/// </summary>
public class DisplayStoryText : MonoBehaviour
{
    [TextArea][SerializeField] private List<string> storyText = default;
    [SerializeField] private string buttonToClose = "Story";

    private Text textObject;
    private int storyIndex;
    
    private void Awake()
    {
        textObject = GetComponentInChildren<Text>();
        textObject.text = string.Empty;
        storyIndex = 0;
    }

    private void OnEnable()
    {
        textObject.text = string.Empty;
    }

    private void Update()
    {
        DisplayText();
    }

    private void DisplayText()
    {
        PlayerMoveRigidbody.HaltMovement = true;
        textObject.text = storyText[storyIndex];
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
