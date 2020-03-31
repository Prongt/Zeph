using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheck : MonoBehaviour
{
    [SerializeField] private GameObject tutorialButton = default;
    [SerializeField] private GameObject forestButton = default;
    [SerializeField] private GameObject snowButton = default;

    [SerializeField] private List<GameObject> mosaicPieces = default;


    private int levelNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        levelNum = GameObject.Find("Player Progress").GetComponent<LevelProgress>().playerProgress;
        
//        print(levelNum);
        if (levelNum == 0)
        {
            tutorialButton.SetActive(true);
        }
        if (levelNum == 1)
        {
            snowButton.SetActive(true);
            //mosaicPieces[1].SetActive(true);
        }
        if (levelNum == 2)
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
