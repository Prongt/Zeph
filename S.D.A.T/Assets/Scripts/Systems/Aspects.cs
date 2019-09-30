using System;
using UnityEngine;

public abstract class Aspects : MonoBehaviour
{
    public virtual void Initialize()
    {
    }

    public virtual void Activate()
    {
        
    }
}

[Serializable]
public enum AspectTypes
{
    Flamable,
    Growable
}