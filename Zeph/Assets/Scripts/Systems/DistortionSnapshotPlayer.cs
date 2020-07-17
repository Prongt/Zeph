using FMODUnity;
using UnityEngine;

/// <summary>
/// Applies an audio distortion to all sound when a distortion event is active
/// </summary>
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
