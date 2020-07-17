using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// When activated the object lerps into specified position
/// </summary>
public class Pullable : Aspects
{
    public Type[] componentTypes =
    {
    };

    [SerializeField] private Transform movePosition = default;
    [SerializeField] private float moveTime = 2.5f;

    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.Pullable;
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