using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class IceController : MonoBehaviour
{
    private Renderer meshRenderer;

    private float desiredValue = -4.7f;
    private float valueToSet;
    private float lerpTime = 1;
    [SerializeField] private float freezeTime;
    [SerializeField] private float meltTime;
    private static readonly int iceLevel = Shader.PropertyToID("iceLevel");
    private Collider col;
    

    private void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
        if (!meshRenderer)
        {
            meshRenderer = GetComponentInChildren<Renderer>();
        }

        col = GetComponent<Collider>();
    }

    private void SetIceOverTime(float value, float time)
    {
        desiredValue = value;
        lerpTime = time;
    }

    private void Update()
    {
        valueToSet = math.lerp(meshRenderer.material.GetFloat(iceLevel), desiredValue, lerpTime * Time.deltaTime);
        meshRenderer.material.SetFloat(iceLevel, valueToSet);
    }

    [ContextMenu("Melt")]
    public void Melt()
    {
        SetIceOverTime(5.2f, meltTime);
        col.isTrigger = true;
        //Debug.Log("Melt");
    }

    [ContextMenu("Freeze")]
    public void Freeze()
    {
        SetIceOverTime(-4.7f, freezeTime);
        col.isTrigger = false;
    }
}
