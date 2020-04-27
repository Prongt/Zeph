using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MosaicPickUp : MonoBehaviour
{
    public LevelProgress myProg;
    // Start is called before the first frame update
    void Start()
    {
        myProg = GameObject.Find("Player Progress").GetComponent<LevelProgress>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            print("Entered Collider");
            if (myProg != null)
            {
                if (myProg.playerProgress < 3)
                {
                    if (SceneManager.GetActiveScene().name == "Ending_Tutorial")
                    {
                        myProg.playerProgress = 1;    
                    }

                    if (SceneManager.GetActiveScene().name == "Ending_Snow")
                    {
                        myProg.playerProgress = 2;
                    }

                    if (SceneManager.GetActiveScene().name == "Ending_ForestPuzzle")
                    {
                        myProg.playerProgress = 3;
                    }
                    
                    //myProg.playerProgress += 1;
                    print(myProg.playerProgress);
                    myProg.SaveLevel();
                }
            }
        }
    }
}
