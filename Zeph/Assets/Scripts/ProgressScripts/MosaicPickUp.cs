using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Updates players current progress
/// </summary>
public class MosaicPickUp : MonoBehaviour
{
    public LevelProgress playerProgress;
    // Start is called before the first frame update
    void Start()
    {
        playerProgress = GameObject.Find("Player Progress").GetComponent<LevelProgress>();
    }


    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            //print("Entered Collider");
            if (playerProgress != null)
            {
                if (playerProgress.playerProgress < 3)
                {
                    if (SceneManager.GetActiveScene().name == "Ending_Tutorial")
                    {
                        playerProgress.playerProgress = 1;    
                    }

                    if (SceneManager.GetActiveScene().name == "Ending_Snow")
                    {
                        playerProgress.playerProgress = 2;
                    }

                    if (SceneManager.GetActiveScene().name == "Ending_ForestPuzzle")
                    {
                        playerProgress.playerProgress = 3;
                    }
                    
                    //myProg.playerProgress += 1;
                    //print(myProg.playerProgress);
                    playerProgress.SaveLevel();
                }
            }
        }
    }
}
