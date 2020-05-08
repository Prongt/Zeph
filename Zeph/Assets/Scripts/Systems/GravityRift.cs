using System;
using UnityEngine;

public class GravityRift : Aspects
{
    public static bool AltGravityIsActive;
    private readonly Vector3 defaultGravity = new Vector3(0, -9.81f, 0);

    public Type[] componentTypes =
    {
    };

    [SerializeField] private Vector3 fromGravity = new Vector3(0, -9.81f, 0);
    [SerializeField] private Vector3 toGravity = new Vector3(0, 0, -9.81f);


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
    
    public override void Promote(Transform source = null, Element element = null)
    {
        if (isCoolDownComplete == false)
        {
            return;
        }

        base.Promote(source, element);

        ChangeGravity(Physics.gravity);
    }

    private void ChangeGravity(Vector3 currentGravity)
    {
        if (currentGravity.Equals(fromGravity))
        {
            Physics.gravity = toGravity;
        }
        else if (currentGravity.Equals(toGravity))
        {
            Physics.gravity = fromGravity;
        }
        
        AltGravityIsActive = !currentGravity.Equals(defaultGravity);
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
        //Not pushed
        //Debug.Log("Not being pushed");
    }
}