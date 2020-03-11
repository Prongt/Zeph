using UnityEngine;
using UnityEngine.Events;

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
