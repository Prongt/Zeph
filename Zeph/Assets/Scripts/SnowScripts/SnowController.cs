using Unity.Mathematics;
using UnityEngine;

public class SnowController : MonoBehaviour
{
    private Renderer meshRenderer;
    private static readonly int snowSize = Shader.PropertyToID("SnowSize");

    private float desiredValue = -1;
    private float valueToSet;
    private float lerpTime = 1;
    [SerializeField] private float freezeTime = 0.25f;
    [SerializeField] private float meltTime = 2;
    
    private void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
        if (!meshRenderer)
        {
            meshRenderer = GetComponentInChildren<Renderer>();
        }
    }

    private void SetSnowSizeOverTime(float value, float time)
    {
        desiredValue = value;
        lerpTime = time;
    }

    private void Update()
    {
        valueToSet = math.lerp(meshRenderer.material.GetFloat(snowSize), desiredValue, lerpTime * Time.deltaTime);
        meshRenderer.material.SetFloat(snowSize, valueToSet);
    }

    [ContextMenu("Melt")]
    public void Melt()
    {
        SetSnowSizeOverTime(1, meltTime);
        //Debug.Log("Melt");
    }

    [ContextMenu("Freeze")]
    public void Freeze()
    {
        SetSnowSizeOverTime(-1, freezeTime);
    }
}
