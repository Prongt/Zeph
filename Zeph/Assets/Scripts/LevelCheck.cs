using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheck : MonoBehaviour
{
    [SerializeField] private GameObject tutorialButton;
    [SerializeField] private GameObject forestButton;
    [SerializeField] private GameObject snowButton;
    [SerializeField] private GameObject caveButton;
    [SerializeField] private GameObject menuButton;

    [SerializeField] private List<GameObject> mosaicPieces;


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
        if (levelNum >= 0)
        {
            tutorialButton.SetActive(true);
        }
        if (levelNum >= 1)
        {
            caveButton.SetActive(true);
            mosaicPieces[0].SetActive(true);
        }
        if (levelNum >= 1)
        {
            snowButton.SetActive(true);
            mosaicPieces[1].SetActive(true);
        }
        if (levelNum >= 2)
        {
            forestButton.SetActive(true);
            mosaicPieces[2].SetActive(true);
        }

        if (levelNum >= 4)
        {
            mosaicPieces[3].SetActive(true);
        }
    }
}
