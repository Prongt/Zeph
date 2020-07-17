using System.IO;
using UnityEngine;

/// <summary>
/// Manages the loading and saving of game files
/// </summary>
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
    
    void Start()
    {
        Time.timeScale = 1;
        print(path);
        DontDestroyOnLoad(gameObject);
        print(Application.persistentDataPath);
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
