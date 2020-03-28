using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndingTimelineEventTrigger : MonoBehaviour
{
    public PlayableDirector timeline;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            Debug.Log("LOG LINE IS WORKING");
            timeline.Play();
        }
    }
}
