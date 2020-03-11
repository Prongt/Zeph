using FMODUnity;
using UnityEngine;

public class TriggerMusic : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter studioEventEmitter;
    [SerializeField] private bool dontDestroyOnLoad = default;
    public static bool MusicIsActive;
    private void Start()
    {
        // if (MusicIsActive)
        // {
        //     return;
        // }
        //
        // MusicIsActive = true;
        GrabComponents();
        studioEventEmitter.Play();
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
    


    private void OnValidate()
    {
        GrabComponents();
    }

    private void GrabComponents()
    {
        if (studioEventEmitter == null)
        {
            studioEventEmitter = GetComponent<StudioEventEmitter>();
        }
    }
}
