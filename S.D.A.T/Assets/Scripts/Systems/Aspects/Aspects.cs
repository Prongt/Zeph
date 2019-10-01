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
        Enum.TryParse(GetType().Name, out AspectType aspectType);
        //Debug.Log(aspectType);
        AspectType = aspectType;
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