using System;
using Unity.Mathematics;
using UnityEngine;

public class MiniBridgeGrow : Aspects
{
    private Renderer meshRenderer;

    private float desiredValue = 11.47f;
    private float valueToSet;
    private float lerpTime = 1;
    [SerializeField] private float unGrownXValue = 11.47f;
    [SerializeField] private float growSpeed = 0.5f;
    [SerializeField] private float grownXValue = -1f;
    private static readonly int vector1D0Babf75 = Shader.PropertyToID("iceLevel");


    private void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
        if (!meshRenderer)
        {
            meshRenderer = GetComponentInChildren<Renderer>();
        }

        desiredValue = unGrownXValue;
    }

    public Type[] componentTypes =
    {
        typeof(FireflyController)
    };
    
    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        //SetGrow(grownXValue, growSpeed);
        print("Promoted");
    }

    public override void Negate(Transform source = null)
    {
        base.Negate(source);
    }

    private void SetGrow(float value, float time)
    {
        desiredValue = value;
        lerpTime = time;
    }

    private void Update()
    {
        valueToSet = math.lerp(meshRenderer.material.GetFloat(vector1D0Babf75), desiredValue, lerpTime * Time.deltaTime);
        meshRenderer.material.SetFloat(vector1D0Babf75, valueToSet);
    }

    [ContextMenu("Grow")]
    public void Grow()
    {
        SetGrow(grownXValue, growSpeed);
    }

}