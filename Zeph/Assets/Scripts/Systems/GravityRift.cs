using System;
using UnityEngine;

public class GravityRift : Aspects
{
    public static bool UseNewGravity;
    private readonly Vector3 defaultGravity = new Vector3(0, -9.81f, 0);

    public Type[] componentTypes =
    {
    };

    [SerializeField] private Vector3 fromGravity;
    [SerializeField] private Vector3 toGravity;


    private void Awake()
    {
        //Debug.LogWarning("Hard setting gravity to default gravity");
        Physics.gravity = defaultGravity;
    }

    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.GravityRift;
    }

    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    private void Update()
    {
        if (Physics.gravity == defaultGravity)
        {
            UseNewGravity = false;
        }
    }


    public override void Promote(Transform source = null, Element element = null)
    {
        if (isCoolDownComplete == false)
        {
            //Debug.Log("Cool down not complete on ", gameObject);
            return;
        }

        base.Promote(source, element);

        ChangeGravity();
    }

    private void ChangeGravity()
    {
        if (Physics.gravity.Equals(toGravity))
        {
            UseNewGravity = true;
            Physics.gravity = fromGravity;
            return;
        }

        if (Physics.gravity.Equals(fromGravity))
        {
            UseNewGravity = true;
            Physics.gravity = toGravity;
        }

        if (Physics.gravity.Equals(defaultGravity)) UseNewGravity = false;
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
        //Not pushed
        //Debug.Log("Not being pushed");
    }
}