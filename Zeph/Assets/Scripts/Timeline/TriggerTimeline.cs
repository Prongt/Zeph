using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

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
