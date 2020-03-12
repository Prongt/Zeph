using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfineController : MonoBehaviour
{
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3	(player.transform.position.x, player	);
    }
}
