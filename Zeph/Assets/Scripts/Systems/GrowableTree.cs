using UnityEngine;

public class GrowableTree : Growable
{
    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.GrowableTree;

        colliders.small.isTrigger = false;
        colliders.distort.enabled = false;
        colliders.grown.enabled = false;
    }

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        Animator.SetBool(grow, true);
    }

    private void Update()
    {
        Animator.SetBool(distort, Distortion.IsDistorting);

        if (gameObject.CompareTag("VerticalSlice"))
        {
            if (Animator.GetBool(distort) && Animator.GetBool(grow))
            {
                colliders.small.isTrigger = true;
                colliders.distort.enabled = true;
                colliders.grown.enabled = false;
            }

            if (Animator.GetBool(distort) == false && Animator.GetBool(grow))
            {
                colliders.small.isTrigger = true;
                colliders.distort.enabled = false;
                colliders.grown.enabled = true;
            }

            return;
        }
        
        if (Distortion.IsDistorting)
        {
            Animator.SetBool(distort, true);
            colliders.small.isTrigger = true;
            colliders.distort.enabled = true;
            colliders.grown.enabled = false;
        }
        else
        {
            Animator.SetBool(distort, false);
            colliders.small.isTrigger = true;
            colliders.distort.enabled = false;
            colliders.grown.enabled = true;
        }
    }
}