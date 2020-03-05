using UnityEngine;


public class Elevator : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private float height;
    public Vector3 endPos;
    public bool rising = true;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = transform.position + Vector3.up * height;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y > endPos.y-0.5f)
        {
            rising = false;
        }
        else if(transform.position.y < startPos.y + 0.5f)
        {
            rising = true;
        }

        if (rising)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, 0.5f * Time.deltaTime);
        }
        else if (!rising)
        {
            transform.position = Vector3.Lerp(transform.position, startPos, 0.5f * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        
        Gizmos.DrawWireCube(transform.position + Vector3.up * height, new Vector3(5,0.1f,5));
    }
}
