using UnityEngine;

/// <summary>
/// Controls the camera confines regardless of player orientation 
/// </summary>
public class CameraConfinesController : MonoBehaviour
{
    private Transform player;
    public float zOffset = 10;
    public float yOffset;
    public float xOffset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Sets camera confine position
    void Update()
    {
        gameObject.transform.position = new Vector3	(player.transform.position.x + xOffset, player.transform.position.y + yOffset, player.transform.position.z + zOffset);
    }

    //Flips how much the Z value is offset from the player
    public void ChangeZOffset()
    {
        zOffset = -zOffset;
    }

    //Flips how much the X value is offset from the player
    public void ChangeXOffset()
    {
        xOffset = -xOffset;
    }

    //Flips how much the Y value is offset from the player
    public void ChangeYOffset()
    {
        yOffset = -yOffset;
    }
}
