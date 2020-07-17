using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Plays timeine when player enters trigger
/// </summary>
public class EndingTimelineEventTrigger : MonoBehaviour
{
    public PlayableDirector timeline;
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            timeline.Play();
        }
    }
}
