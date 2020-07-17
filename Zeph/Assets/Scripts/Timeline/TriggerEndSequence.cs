using Movement;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// When player enters trigger freeze movement & calls unity event
/// </summary>
public class TriggerEndSequence : MonoBehaviour
{
    public UnityEvent OnPlayerEnter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMoveRigidbody.HaltMovement = true;
            OnPlayerEnter.Invoke();
        }
    }
}
