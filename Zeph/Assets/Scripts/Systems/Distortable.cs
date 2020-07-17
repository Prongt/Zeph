using System;
using UnityEngine;

/// <summary>
/// When activated a morph animation is triggered and the collider is changed
/// </summary>
public class Distortable : MonoBehaviour
{
    [SerializeField] private Animator myAnim;
    [SerializeField] private Colliders colliders = default;
    private static readonly int distort = Animator.StringToHash("Distort");
    

    private void Start()
    {
        myAnim = GetComponentInChildren<Animator>();
        
        if (colliders.normal && colliders.distort)
        {
            colliders.normal.enabled = true;
            colliders.distort.enabled = false;
        }
    }

    private void Update()
    {
        //Static bool set from the distortion script on the distortion rifts
        if (Distortion.IsDistorting)
        {
            if (colliders.normal && colliders.distort)
            {
                colliders.distort.enabled = true;
                colliders.normal.enabled = false;
            }
        }
        else
        {
            if (colliders.normal && colliders.distort)
            {
                colliders.normal.enabled = true;
                colliders.distort.enabled = false;
            }
        }
        
        myAnim.SetBool(distort, Distortion.IsDistorting);
    }
    
    [Serializable]
    private struct Colliders
    {
        public Collider normal;
        // ReSharper disable once MemberHidesStaticFromOuterClass
        public Collider distort;
    }
}
