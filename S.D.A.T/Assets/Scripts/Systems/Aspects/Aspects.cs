using System;
using UnityEngine;

public abstract class Aspects : MonoBehaviour
{
    [HideInInspector] public AspectType AspectType;

    private void Awake()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
    }

    public virtual void Activate()
    {
        
    }

    public abstract void Promote();
    public abstract void Negate();

}



[Serializable]
public enum AspectType
{
    Flamable,
    Growable,
    Pushable
}