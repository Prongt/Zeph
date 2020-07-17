using UnityEngine;

/// <summary>
/// Sets object active state to match distortion state
/// </summary>
public class EnableDuringDistortion : MonoBehaviour
{
    [SerializeField] private GameObject objectToDisable;
    void Update()
    {
        objectToDisable.SetActive(Distortion.IsDistorting);
    }
}
