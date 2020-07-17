using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables or disables levels in the level select screen if level is not currently accessible to the player
/// </summary>
public class LevelCheck : MonoBehaviour
{
    [SerializeField] private GameObject tutorialButton = default;
    [SerializeField] private GameObject forestButton = default;
    [SerializeField] private GameObject snowButton = default;

    [SerializeField] private List<GameObject> mosaicPieces = default;


    private int levelNum;
    void Start()
    {
        levelNum = GameObject.Find("Player Progress").GetComponent<LevelProgress>().playerProgress;
    }
    
    void Update()
    {
        //levelNum = GameObject.Find("Player Progress").GetComponent<LevelProgress>().playerProgress;
        
//        print(levelNum);
        if (levelNum == 0)
        {
            tutorialButton.SetActive(true);
        }
        if (levelNum >= 1)
        {
            snowButton.SetActive(true);
            //mosaicPieces[1].SetActive(true);
        }
        if (levelNum >= 2)
        {
            forestButton.SetActive(true);
            //mosaicPieces[2].SetActive(true);
        }

        if (levelNum == 4)
        {
            mosaicPieces[3].SetActive(true);
        }
    }
}
