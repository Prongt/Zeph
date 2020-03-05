using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//For the Stalactite. Checks if its hit by a rock and falls.
public class Break : MonoBehaviour
{
    private Rigidbody myRB;
    
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rock"))
        {
            myRB.useGravity = true;
        }
    }
}
