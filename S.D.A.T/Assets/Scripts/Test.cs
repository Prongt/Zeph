using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Test");
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Hey");
        }
    }
}