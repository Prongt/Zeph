using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgress : MonoBehaviour
{

    public int playerProgress;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        print(Application.persistentDataPath);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveLevel()
    {
        SaveSystem.SaveLevel(this);
    }

    public void LoadLevel()
    {
        LevelData level = SaveSystem.LoadLevel();
        playerProgress = level.level;
    }

    private void OnApplicationQuit()
    {
        SaveLevel();
    }
}
