using System;
using UnityEngine;

public abstract class Aspects : MonoBehaviour
{
    [HideInInspector] public AspectType AspectType;
    private void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        Enum.TryParse(GetType().Name, out AspectType aspectType);
        //Debug.Log(aspectType);
        AspectType = aspectType;
    }

    public abstract void Promote(Transform source = null);
    public abstract void Negate(Transform source = null);
}


[Serializable]
public enum AspectType
{
    Flamable,
    Growable,
    Pushable
}