using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder.MeshOperations;

public class MirrorBeam : MonoBehaviour
{
    [SerializeField] private UnityEvent onActivate = default;

    public GameObject crystal;
    
    // Start is called before the first frame update
    void Start()
    {
        //crystal = gameObject.transform.Find("Crystal Light").gameObject;
        if (crystal != null)
        {
            Debug.Log("Crystal Found");
        }
        else
        {
            crystal = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        OnActivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            crystal = other.gameObject;
        }
    }

    public virtual void OnActivate()
    {
        if (crystal != null)
        {
            if (crystal.GetComponent<Chargeable>().charged && crystal.GetComponent<Chargeable>().attached)
            {
                onActivate.Invoke();
            }
        }
    }
}
