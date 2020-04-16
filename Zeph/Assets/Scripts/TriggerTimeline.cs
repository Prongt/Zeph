using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TriggerTimeline : MonoBehaviour
{
    [SerializeField]private PlayableDirector timeline;
    [SerializeField]private bool triggerOnce = true;

    private bool triggered = false;


    public void PlayTimeline()
    {
        if (triggerOnce)
        {
            if (triggered) return;
        }
        timeline.Play();
        triggered = true;
    }
}
