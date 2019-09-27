using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Aspects : MonoBehaviour
{
    public virtual void Initialize()
    {
        
    }
}

[Serializable]
public enum AspectTypes
{
    Flamable,
    Wetable,
    Growable,
    Damagable,
    Throwable
}
