using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads specified scene when menu button is selected or when player enters loading area
/// </summary>
public class LoadScene : MonoBehaviour
{
    [SerializeField] private string endingToLoad;
    private bool transLoad;
    public void SceneLoad(String sceneToLoad)
    {
        if (GameObject.Find("Player Progress").GetComponent<LevelProgress>().playerProgress == 0)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            SceneManager.LoadScene("Hub");
        }
    }

    public void MapLoad(String sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        
    }

    void Start()
    {
        transLoad = false;
    }

    void Update()
    {
        if (transLoad)
        {
            MapLoad(endingToLoad);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Colliding");
        if (gameObject.CompareTag("Arch"))
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene(endingToLoad);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transLoad = true;
        }
    }
}
