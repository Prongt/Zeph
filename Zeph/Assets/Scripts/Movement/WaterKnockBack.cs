using System;
using FMODUnity;
using UnityEngine;

namespace Movement
{

    public class WaterKnockBack : MonoBehaviour
    {
        [SerializeField] private bool applyKnockBackForce = false;
        [HideIf("applyKnockBackForce", true)][SerializeField] private float forceAmount = 5f;
        [SerializeField] private bool teleportPlayer = false;
        [HideIf("teleportPlayer", true)][SerializeField] private Transform teleportPosition = default;
        [EventRef][SerializeField] private string fmodEvent = default;
        [SerializeField] private bool disableDuringDistort = false;

        private PlayerMoveRigidbody playerMoveRigidbody;
        private Collider collider;

        private void Start()
        {
            playerMoveRigidbody = FindObjectOfType<PlayerMoveRigidbody>();
            collider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (!disableDuringDistort) return;
            collider.isTrigger = Distortion.IsDistorting;
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
        
            RuntimeManager.PlayOneShot(fmodEvent, col.contacts[0].point);
        }
        
    }
}
