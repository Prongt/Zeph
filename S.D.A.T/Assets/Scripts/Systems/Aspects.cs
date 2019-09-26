using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aspects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}

[Serializable]
public enum AspectTypes
{
    Burnable,
    Wetable,
    Growable,
    Damagable,
    Throwable
    
}
