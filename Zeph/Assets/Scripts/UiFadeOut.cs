using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiFadeOut : MonoBehaviour
{
    
    [SerializeField] private float fadeTime = 2.0f;

    private MaskableGraphic[] guiElements;
    private float[] alphas;

    
    private void Awake()
    {
        guiElements = GetComponentsInChildren<MaskableGraphic>();
        alphas = new float[guiElements.Length];
        
        
        for (int i = 0; i < alphas.Length; i++)
        {
            alphas[i] = guiElements[i].color.a;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            FadeUi();
        }
    }


    public void FadeUi()
    {
        //Debug.Log("No");
        gameObject.SetActive(false);

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
