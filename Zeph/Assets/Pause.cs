using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private GameObject options;
    private bool menuActive;
    private bool optionsActive;
    
    // Start is called before the first frame update
    void Start()
    {
        menuActive = false;
    }

    // Update is called once per frame
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
            print("PAUSED");
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
        SceneManager.LoadScene(sceneToLoad);
    }
}
