using UnityEngine;

/// <summary>
/// Hides mouse cursor at the start of the scene
/// </summary>
public class HideMouse : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
