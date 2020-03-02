using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;

public class PlayDanceAnimation : MonoBehaviour
{
    private PlayerMove playerMove;
    void Start()
    {
        playerMove = FindObjectOfType<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dance()
    {
        playerMove.StartCoroutine(playerMove.UseDanceAnimation());
    }
}
