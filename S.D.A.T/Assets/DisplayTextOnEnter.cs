using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTextOnEnter : MonoBehaviour
{

    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");

        if (other.GetComponent<PlayerMove>())
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMove>())
        {
            //transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
