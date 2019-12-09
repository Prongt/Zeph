using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFadeOut : MonoBehaviour
{
    [SerializeField] private List<string> storyText;
    [SerializeField] private float fadeTime = 2.0f;

    private MaskableGraphic[] guiElements;
    private float[] alphas;

    private Text text;
    private int storyIndex = 0;
    
    private void Awake()
    {
        guiElements = GetComponentsInChildren<MaskableGraphic>();
        alphas = new float[guiElements.Length];
        text = GetComponentInChildren<Text>();
        
        for (int i = 0; i < alphas.Length; i++)
        {
            alphas[i] = guiElements[i].color.a;
        }

        storyIndex = 0;

    }

    private void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            FadeUi();
        }
        DisplayText();
    }

    private void DisplayText()
    {
        PlayerMove._PlayerMovementEnabled = false;
        text.text = storyText[storyIndex];
        if (Input.GetButtonDown("Story"))
        {
            if (storyIndex == storyText.Count -1)
            {
                FadeUi();
                return;
            }
            storyIndex++;
        }
    }


    public void FadeUi()
    {
        //Debug.Log("No");
        gameObject.SetActive(false);
        storyIndex = 0;
        PlayerMove._PlayerMovementEnabled = true;

//        foreach (var element in guiElements)
//        {
//            StartCoroutine(FadeRoutine(element, fadeTime));
//        }
    }

    private IEnumerator FadeRoutine(MaskableGraphic element, float time)
    {
//        for (float t = 0f; t < time; t += Time.deltaTime)
//        {
//            float normalizedTime = t / time;
//            Color dss = element.color;
//            dss.a = 0;
//
//            element.color = Color.Lerp(element.color, dss, normalizedTime);
//            yield return null;
//        }

        var col = element.color;
        while (col.a > 0.01f)
        {
            col.a = Mathf.Lerp(element.color.a, 0, time * Time.deltaTime);
            element.color = col;

            yield return null;
        }

        //Debug.Log("Done");
        DeActivateUi();

        yield return 0;
    }

    private void DeActivateUi()
    {
        for (int i = 0; i < guiElements.Length; i++)
        {
//          guiElements[i].gameObject.SetActive(false);
            gameObject.SetActive(false);
            var col = guiElements[i].color;
            col.a = alphas[i];
            guiElements[i].color = col;
        }
    }
}
