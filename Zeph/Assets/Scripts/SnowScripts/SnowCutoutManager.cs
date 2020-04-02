using UnityEngine;

public class SnowCutoutManager : MonoBehaviour
{
    [SerializeField] private bool useSpecifiedConductor = true;
    [HideIf("useSpecifiedConductor", true)] [SerializeField] private GameObject conductor;
    private Material material;
    private static readonly int position = Shader.PropertyToID("Position");
    private Transform tempConductor;
   
    private void Start()
    {
        material = gameObject.GetComponent<Renderer>().material;
        tempConductor = useSpecifiedConductor ? conductor.transform : transform;
    }

    private void Update()
    {
        if (!useSpecifiedConductor)
        {
            if (Conductor.GlobalConductor)
            {
                tempConductor = Conductor.GlobalConductor.transform;
            }
        }
        
        material.SetVector(position,  
        new Vector4(tempConductor.position.x, tempConductor.position.y, tempConductor.position.z, 
        tempConductor.transform.localScale.x / 2));
    }
}