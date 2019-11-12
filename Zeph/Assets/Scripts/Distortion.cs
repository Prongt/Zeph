using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Distortion : MonoBehaviour
{
    [SerializeField] private Animator disAnim1;
    [SerializeField] private Animator disAnim2;

    private bool animating = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animating = !animating;
            print("doing");
            disAnim1.SetBool("Animate", animating);
            disAnim2.SetBool("Animate", animating);
        }
    }
}
