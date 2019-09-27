using System;
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
    Growable
}