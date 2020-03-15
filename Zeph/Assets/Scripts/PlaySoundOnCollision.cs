using System;
using Cinemachine;
using FMODUnity;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    [SerializeField] [EventRef] private string collisionEvent;
    [SerializeField] [TagField] private string[] tagsToIgnore;
    [SerializeField] private bool playOnce;
    [SerializeField] private Transform audioPoint;
    private bool hasPlayed;
    

    private void OnCollisionEnter(Collision other)
    {
        for (int i = 0; i < tagsToIgnore.Length; i++)
        {
            if (other.collider.CompareTag(tagsToIgnore[i])) return;

            if (hasPlayed) return;
           
            if (playOnce) hasPlayed = true;
 
            RuntimeManager.PlayOneShot(collisionEvent, audioPoint ? audioPoint.position : transform.position);
        }
    }
}
