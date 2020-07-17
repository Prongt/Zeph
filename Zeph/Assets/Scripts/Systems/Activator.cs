using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Calls a unity event in the inspector when the player enters a trigger
/// </summary>
public class Activator : MonoBehaviour
{
    [SerializeField] private UnityEvent onPlayerEnter = default;

    public virtual void Activate()
    {
        
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            onPlayerEnter.Invoke();
            Activate();
        }
    }
}
