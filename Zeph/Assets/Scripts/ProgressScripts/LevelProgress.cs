using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;


public class LevelProgress : MonoBehaviour
{

    public int playerProgress;
    private string path;

    private void Awake()
    {
       path = Application.persistentDataPath + "/level.grannus";
       if (File.Exists(path))
       {
            print("loading");
            LoadLevel();
       }
       else
       {
            print("saving");
            SaveLevel();
       }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        print(path);
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
