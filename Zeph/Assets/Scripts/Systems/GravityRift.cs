using System;
using UnityEngine;

public class GravityRift : Aspects
{
    [SerializeField] private Vector3 newGravity;
    [SerializeField] private bool useToFromGravity;
    [SerializeField] private Vector3 toGravity;
    [SerializeField] private Vector3 fromGravity;
    private static Vector3 ogGravity = new Vector3(0, -9.81f, 0);

    public static bool UseNewGravity = false;

    public bool resetGravity = true;

    void Awake()
    {
        //Debug.LogWarning("Hard setting gravity to default gravity");
        Physics.gravity = new Vector3(0, -9.81f, 0);
        ogGravity = Physics.gravity;
    }

    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.GravityRift;
    }

    public Type[] componentTypes = new Type[]
    {
        
    };

    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    
    public override void Promote(Transform source = null, Element element = null)
    {
        if (isCoolDownComplete == false)
        {
            Debug.Log("Cool down not complete on " + gameObject.name);
            return;
        }
        base.Promote(source, element);
        
        ChangeGravity();
    }
    
    private void ChangeGravity()
    {
        if (useToFromGravity)
        {
            if (Physics.gravity.Equals(toGravity))
            {
//                Debug.Log(fromGravity);
                UseNewGravity = true;
                Physics.gravity = fromGravity;
                return;
            }
            
            if (Physics.gravity.Equals(fromGravity))
            {
                //Debug.Log(toGravity);
                UseNewGravity = true;
                Physics.gravity = toGravity;
                return;
            }
        }
    
    
        if (!resetGravity)
        {
            Physics.gravity = newGravity;
        }
        else
        {
            if (!UseNewGravity)
            {
                UseNewGravity = true;
                Physics.gravity = newGravity;
            }
            else
            {
                UseNewGravity = false;
                Physics.gravity = ogGravity;
            }
        }
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
        //Not pushed
        //Debug.Log("Not being pushed");
    }
    
        
}
