using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour
{
    public String levelToLoad;
    
    void Start()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
