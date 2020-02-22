﻿using System.Collections;
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
            if (myProg.playerProgress < 4)
            {
                myProg.playerProgress += 1;
                SceneManager.LoadScene("Hub");
            }
        }
    }
}