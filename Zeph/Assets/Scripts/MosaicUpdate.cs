using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosaicUpdate : MonoBehaviour
{
    public List<GameObject> mosaics;
    private Animator anim;
    private LevelProgress prog;

    void Start()
    {
        anim = GetComponent<Animator>();
        prog = GameObject.Find("Player Progress").GetComponent<LevelProgress>();
    }

    void Update()
    {
        anim.SetInteger("PlayerProgress", prog.playerProgress);
    }

}
