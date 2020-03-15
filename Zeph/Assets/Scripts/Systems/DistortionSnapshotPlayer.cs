using FMODUnity;
using UnityEngine;

public class DistortionSnapshotPlayer : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter distortionEventEmitter;

    private void Start()
    {
        distortionEventEmitter = GetComponent<StudioEventEmitter>();
    }

    private void Update()
    {
        if (Distortion.IsDistorting)
        {
            distortionEventEmitter.Play();
        }
        else
        {
            distortionEventEmitter.Stop();
        }
    }
}
