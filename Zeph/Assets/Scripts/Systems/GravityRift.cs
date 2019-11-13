
using UnityEngine;

public class GravityRift : Activator
{
    [SerializeField] private Vector3 newGravity;
    private static Vector3 ogGravity = new Vector3(0, -9.81f, 0);

    public static bool useNewGravity = false;

    public bool resetGravity = true;
    // Start is called before the first frame update
    void Awake()
    {
        ogGravity = Physics.gravity;
    }


    public override void Activate()
    {
        if (!resetGravity)
        {
            Physics.gravity = newGravity;
        }
        else
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
}
