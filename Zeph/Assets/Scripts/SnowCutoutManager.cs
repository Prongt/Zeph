using UnityEngine;

public class SnowCutoutManager : MonoBehaviour
{
    public GameObject conductor;
    private Material material;
    public float radius = 0.5f;
    private static readonly int position = Shader.PropertyToID("Position");

    private void Start()
    {
        material = gameObject.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        var conductorPos = conductor.transform.position;
        material.SetVector(position,  
        new Vector4(conductorPos.x, conductorPos.y, conductorPos.z, 
        conductor.transform.localScale.x / 2));
    }
}