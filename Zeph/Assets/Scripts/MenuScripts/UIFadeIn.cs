using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Fades the alpha of the child UI elements on scene start
/// </summary>
public class UIFadeIn : MonoBehaviour
{
    private MaskableGraphic[] guiElements;
    public float fadeTime = 2.0f;
    public float fadeSpeed = 0.1f;

    private void Start()
    {
        guiElements = GetComponentsInChildren<MaskableGraphic>();
        
//        for (int i = 0; i < guiElements.Length; i++)
//        {
//            var col = guiElements[i].color;
//            col.a = 0;
//
//            guiElements[i].color = col;
//
//            StartCoroutine(FadeRoutine(guiElements[i], fadeTime));
//        }

        
        StartCoroutine(LoadScene());
    }

    private void Update()
    {
        for (int i = 0; i < guiElements.Length; i++)
        {
            var col = guiElements[i].color;
            col.a += Time.deltaTime * fadeSpeed;

            guiElements[i].color = col;

            //StartCoroutine(FadeRoutine(guiElements[i], fadeTime));
        }
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(fadeTime + 0.5f);
        SceneManager.LoadScene("MenuV2");
    }
    

    private IEnumerator FadeRoutine(MaskableGraphic element, float time)
    {
        var col = element.color;
        while (col.a < 250f)
        {
            col.a = Mathf.Lerp(0, 255f, time * Time.deltaTime);
            element.color = col;

            yield return null;
        }


        

        yield return 0;
    }

}
