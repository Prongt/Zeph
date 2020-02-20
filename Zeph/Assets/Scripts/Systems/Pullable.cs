using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Pullable : Aspects
{
    public Type[] componentTypes =
    {
    };

    [SerializeField] private Transform movePosition;
    [SerializeField] private float moveTime;

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
    private void StartMovement()
    {
        StopCoroutine(MoveRoutine());
        StartCoroutine(MoveRoutine());
    }


    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);

        StartMovement();
    }
}