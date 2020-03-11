using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalAttach : MonoBehaviour
{
    [SerializeField] private Transform crystalPos = default;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider crystal)
    {
        if (crystal.CompareTag("Crystal"))
        {
            crystal.gameObject.transform.position = crystalPos.position;
            crystal.gameObject.transform.rotation = crystalPos.rotation;
            crystal.gameObject.transform.localScale = crystalPos.localScale;
            crystal.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            crystal.GetComponent<Chargeable>().attached = true;
        }   
    }
}
