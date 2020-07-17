using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads specified level at the start of a scene
/// </summary>
public class LevelLoad : MonoBehaviour
{
    public String levelToLoad;
    
    void Start()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
