using Cinemachine;
using FMODUnity;
using UnityEngine;

/// <summary>
/// Plays sound at specified location when the gameobject collides with another
/// </summary>
public class PlaySoundOnCollision : MonoBehaviour
{
    [SerializeField] [EventRef] [Tooltip("Audio event to play on collision")] 
    private string audioEvent;
    [SerializeField] [TagField] [Tooltip("Tags that are ignored during collision")] 
    private string[] tagsToIgnore;
    [SerializeField] [Tooltip("If true sound only plays on first collision")]
    private bool playOnce;
    [SerializeField] [Tooltip("Location of audio source")] 
    private Transform audioPoint;
    
    private bool hasPlayed;
    

    private void OnCollisionEnter(Collision other)
    {
        for (int i = 0; i < tagsToIgnore.Length; i++)
        {
            if (other.collider.CompareTag(tagsToIgnore[i])) return;

            if (hasPlayed) return;
           
            if (playOnce) hasPlayed = true;
 
            RuntimeManager.PlayOneShot(audioEvent, audioPoint ? audioPoint.position : transform.position);
        }
    }
}
