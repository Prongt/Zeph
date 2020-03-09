using FMODUnity;
using UnityEngine;

namespace Movement
{
    public class WaterKnockBack : MonoBehaviour
    {
        [SerializeField] private bool applyKnockBackForce;
        [HideIf("applyKnockBackForce", true)][SerializeField] private float forceAmount = 5f;
        [SerializeField] private bool teleportPlayer;
        [HideIf("teleportPlayer", true)][SerializeField] private Transform teleportPosition;
        [EventRef][SerializeField] private string fmodEvent;

        private PlayerMoveRigidbody playerMoveRigidbody;

        private void Start()
        {
            playerMoveRigidbody = FindObjectOfType<PlayerMoveRigidbody>();
        }

        private void OnCollisionEnter(Collision col)
        {
            if (!col.collider.CompareTag("Player")) return;
        
            if (applyKnockBackForce)
            {
                var knockBackVector = col.contacts[0].point - col.transform.position;
                knockBackVector = -knockBackVector.normalized;
            
                playerMoveRigidbody.ApplyKnockBackForce(knockBackVector * forceAmount, ForceMode.Impulse);
            }

            if (teleportPlayer)
            {
                playerMoveRigidbody.TeleportPlayer(teleportPosition);
            }
        
            RuntimeManager.PlayOneShot(fmodEvent, transform.position);
        }
    
    }
}
