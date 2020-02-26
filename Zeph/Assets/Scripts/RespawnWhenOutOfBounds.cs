using Cinemachine;
using UnityEngine;

public class RespawnWhenOutOfBounds : MonoBehaviour
{
    [SerializeField][TagField] private new string tag;
    private Vector3 startPos;
    private Quaternion startRot;
    private Rigidbody rigidBody;

    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tag)) return;
        rigidBody.Sleep();
        transform.SetPositionAndRotation(startPos, startRot);
    }
}
