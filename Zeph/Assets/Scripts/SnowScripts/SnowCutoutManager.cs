using UnityEngine;

public class SnowCutoutManager : MonoBehaviour
{
    [SerializeField] private bool useSpecifiedConductor = true;
    [HideIf("useSpecifiedConductor", true)] [SerializeField] private GameObject conductor;
    [SerializeField] private  float radius = 0.5f;
    private Material material;
    private static readonly int position = Shader.PropertyToID("Position");

    private void Start()
    {
        material = gameObject.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        Vector3 conductorPos;
        if (useSpecifiedConductor)
        {
            conductorPos = conductor.transform.position;
        }
        else
        {
            conductorPos = Conductor.GlobalConductor.position;
        }
        
        material.SetVector(position,  
        new Vector4(conductorPos.x, conductorPos.y, conductorPos.z, 
        conductor.transform.localScale.x / 2));
    }
}