using UnityEngine;

public class GrowablePlant : Growable
{
    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.GrowablePlant;
    }

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        
        Animator.SetBool(growing, true);
        
        if (colliders.grown)
        {
            colliders.grown.enabled = true;
            colliders.grown.isTrigger = false;
        }

        if (colliders.small)
        {
            colliders.small.isTrigger = true;
        }
    }
}