using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDisabler : MonoBehaviour
{
    [SerializeField] private bool disableDuringDistort = false;
    [SerializeField] private bool disableDuringAltGravity = false;
    [SerializeField] private bool enabledDuringAltGravity = false;
    
    private Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (enabledDuringAltGravity)
        {
            collider.isTrigger = GravityRift.AltGravityIsActive == false;
        }
        
        if (disableDuringDistort)
        {
            if (Distortion.IsDistorting)
            {
                collider.isTrigger = true;
                return;
            }
            else
            {
                collider.isTrigger = false;
            }
        }
        if (disableDuringAltGravity)
        {
            if (GravityRift.AltGravityIsActive)
            {
                collider.isTrigger = true;
                return;
            }
            else
            {
                collider.isTrigger = false;
            }
        }
        
            
        // if (Distortion.IsDistorting || GravityRift.AltGravityIsActive)
        // {
        //     collider.isTrigger = true;
        // }
        // else
        // {
        //     collider.isTrigger = false;
        // }
    }
}
