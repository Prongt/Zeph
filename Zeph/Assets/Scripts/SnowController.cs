using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SnowController : MonoBehaviour
{
    private Renderer _renderer;
    private static readonly int SnowSize = Shader.PropertyToID("SnowSize");

    private float _desiredValue = -1;
    private float _valueToSet;
    private float _startingValue;
    private float _lerpTime = 1;
    [SerializeField] private float freezeTime;
    [SerializeField] private float meltTime;
    
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (!_renderer)
        {
            _renderer = GetComponentInChildren<Renderer>();
        }
        _startingValue = _renderer.material.GetFloat(SnowSize);
    }

    private void SetSnowSizeOverTime(float value, float time)
    {
        _startingValue = _renderer.material.GetFloat(SnowSize);
        _desiredValue = value;
        _lerpTime = time;
    }

    private void Update()
    {
        
        _valueToSet = math.lerp(_renderer.material.GetFloat(SnowSize), _desiredValue, _lerpTime * Time.deltaTime);
        _renderer.material.SetFloat(SnowSize, _valueToSet);
    }

    [ContextMenu("Melt")]
    public void Melt()
    {
        SetSnowSizeOverTime(1, meltTime);
        Debug.Log("Melt");
    }

    [ContextMenu("Freeze")]
    public void Freeze()
    {
        SetSnowSizeOverTime(-1, freezeTime);
    }
}
