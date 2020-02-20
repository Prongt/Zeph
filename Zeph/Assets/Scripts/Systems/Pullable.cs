using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Pullable : Aspects
{
    [SerializeField] private float moveTime;
    [SerializeField] private Transform movePosition;
    private bool canStartMoving = false;
    
    public Type[] componentTypes = new Type[]
    {
    };

    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    private IEnumerator MoveRoutine()
    {
        while (math.distance(transform.position, movePosition.position) > 0.25f)
        {
            transform.position = Vector3.Lerp(transform.position, movePosition.position, moveTime * Time.deltaTime);
            yield return null;
        }
    }

    [ContextMenu("Move")]
    void Move()
    {
        StopCoroutine(MoveRoutine());
        StartCoroutine(MoveRoutine());
    }
    


    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        
        Move();
        

    }

    public override void Negate(Transform source = null)
    {
        base.Negate(source);
    }
}