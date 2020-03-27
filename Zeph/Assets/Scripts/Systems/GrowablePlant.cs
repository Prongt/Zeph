using UnityEngine;

public class GrowablePlant : Growable
{
    [SerializeField] private bool useTrigger = false;
    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.GrowablePlant;
        
        if (!useTrigger) return;
        if (colliders.small)
        {
            colliders.small.isTrigger = useTrigger;
        }
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
            colliders.small.isTrigger = false;
        }
    }
}