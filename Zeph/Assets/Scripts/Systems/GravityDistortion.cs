using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityDistortion : MonoBehaviour
{
    public Vector3 newGravity;
    private Vector3 ogGravity;

    public static bool useNewGravity = false;
    // Start is called before the first frame update
    void Start()
    {
        ogGravity = Physics.gravity;
    }

    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Q))
//        {
//            Physics.gravity = Vector3.up;
//        }
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            Physics.gravity = Vector3.down;
//        }
//    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Physics.gravity = newGravity;
            useNewGravity = true;
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Physics.gravity = ogGravity;
            useNewGravity = false;
        }
    }
}
