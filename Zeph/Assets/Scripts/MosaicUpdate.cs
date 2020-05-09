using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosaicUpdate : MonoBehaviour
{
    public List<GameObject> mosaics;
    private LevelProgress prog;

    void Start()
    {
        prog = GameObject.Find("Player Progress").GetComponent<LevelProgress>();
    }

    void Update()
    {
        if (prog.playerProgress == 1)
        {
            mosaics[0].SetActive(true);
        }
        if (prog.playerProgress == 2)
        {
            mosaics[1].SetActive(true);
        }
        if (prog.playerProgress == 3)
        {
            mosaics[2].SetActive(true);
            mosaics[0].SetActive(false);
            mosaics[1].SetActive(false);
        }
    }

}
