﻿using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string endingToLoad;
    public void SceneLoad(String sceneToLoad)
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
        if (gameObject.CompareTag("Arch"))
        {
            if (other.CompareTag("Player"))
            {
                SceneLoad(endingToLoad);
            }
        }
    }
}
