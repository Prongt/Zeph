using FMODUnity;
using UnityEngine;

/// <summary>
/// Triggers an fmod event to play at the start of the scene
/// </summary>
public class TriggerMusic : MonoBehaviour
{
    [SerializeField] [Tooltip("Fmod event emitter")]
    private StudioEventEmitter studioEventEmitter;
    
    [SerializeField] [Tooltip("If True this gameobject will persist between scenes")]
    private bool dontDestroyOnLoad = false;
    
    public static bool MusicIsActive;
    private void Start()
    {
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
