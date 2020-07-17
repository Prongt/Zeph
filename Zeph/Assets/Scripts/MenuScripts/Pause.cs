using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Pause menu manager
/// </summary>
public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private GameObject options;
    private bool menuActive;
    private bool optionsActive;
    
    void Start()
    {
        menuActive = false;
    }
    
    void Update()
    {
        if (Input.GetButtonUp("Pause"))
        {
            menuActive = !menuActive;
            if (optionsActive)
            {
                optionsActive = !optionsActive;
                menuActive = false;
            }
        }

        if (menuActive)
        {
            //print("PAUSED");
            Time.timeScale = 0;
                pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }

        if (optionsActive)
        {
            //Time.timeScale = 0;
            options.SetActive(true);
        }
        else
        {
            //Time.timeScale = 1;
            if (options)
                options.SetActive(false);
        }
    }

    public void CloseMenu()
    {
        menuActive = !menuActive;
    }

    public void OptionsMenu(GameObject options)
    {
        this.options = options;
        menuActive = false;
        optionsActive = true;
    }

    public void ResetLevel(string sceneToReset)
    {
        SceneManager.LoadScene(sceneToReset);
    }

    public void ExitToMenu(string sceneToLoad)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneToLoad);
    }
    
}
