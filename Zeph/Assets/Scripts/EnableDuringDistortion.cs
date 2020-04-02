using UnityEngine;

public class EnableDuringDistortion : MonoBehaviour
{
    [SerializeField] private GameObject objectToDisable;
    void Update()
    {
        objectToDisable.SetActive(Distortion.IsDistorting);
    }
}
