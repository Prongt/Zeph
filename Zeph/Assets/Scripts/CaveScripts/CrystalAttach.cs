using UnityEngine;

public class CrystalAttach : MonoBehaviour
{
    [SerializeField] private Transform crystalPos = default;

    private void OnTriggerEnter(Collider crystal)
    {
        if (!crystal.CompareTag("Crystal")) return;
        
        var crystalGameObject = crystal.gameObject;
        crystalGameObject.transform.position = crystalPos.position;
        crystalGameObject.transform.rotation = crystalPos.rotation;
        crystalGameObject.transform.localScale = crystalPos.localScale;
        crystal.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        crystal.GetComponent<Chargeable>().attached = true;
    }
}
