using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string endingToLoad;
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
}
