using UnityEngine;

public class GravityDistortion : Activator
{
    [SerializeField] private Vector3 newGravity;
    private static Vector3 ogGravity = new Vector3(0, -9.81f, 0);

    public static bool useNewGravity = false;
    // Start is called before the first frame update
    void Awake()
    {
        ogGravity = Physics.gravity;
    }


    public override void Activate()
    {
        if (!useNewGravity)
        {
            useNewGravity = true;
            Physics.gravity = newGravity;
        }
        else
        {
            useNewGravity = false;
            Physics.gravity = ogGravity;
        }
    }
}
