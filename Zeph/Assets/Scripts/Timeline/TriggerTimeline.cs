using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Triggers cinemachine timeline
/// </summary>
public class TriggerTimeline : MonoBehaviour
{
    [SerializeField]private PlayableDirector timeline;
    [SerializeField]private bool triggerOnce = true;

    private bool hasBeentriggered = false;


    public void PlayTimeline()
    {
        if (triggerOnce)
        {
            if (hasBeentriggered) return;
        }
        timeline.Play();
        hasBeentriggered = true;
    }
}
