using System;
using System.Collections;
using UnityEngine;

public class Flamable : Aspects
{
    [SerializeField] private Material burnedMaterial;
    [SerializeField] private ParticleSystem burningParticleEffect;

    private Material baseMaterial;
    private bool isSource;
    
    
    public Type[] componentTypes = new Type[]
    {
        typeof(AudioSource)
    };


    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }
    
    
    protected override void Initialize()
    {
        base.Initialize();
        baseMaterial = GetComponent<Renderer>().material;
        burningParticleEffect.Stop();
    }

    public override void Promote(Transform source = null)
    {
        base.Promote(source);
        //Debug.Log("On Fire");
        GetComponent<Renderer>().material = burnedMaterial;
        Instantiate(burningParticleEffect.gameObject, gameObject.transform);
        burningParticleEffect.Play();
        StartCoroutine(Burn());
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
        //Extingushed
        //Debug.Log("Extingushed");
        GetComponent<Renderer>().material = baseMaterial;
    }

    IEnumerator Burn()
    {
        //This is broken
        /*if (burnedMaterial.color.a > 1)
        {
            burnedMaterial.color = Color.Lerp(burnedMaterial.color, new Color(burnedMaterial.color.r, burnedMaterial.color.g, burnedMaterial.color.b, 0), 1 * Time.deltaTime);
        }
        StartCoroutine(Burn());*/
        yield return null;
    }

    private IEnumerator FireSpread()
    {
//        elementData[i].colliders = new Collider[maxAffectableObjects];
//        var size = Physics.OverlapSphereNonAlloc(transform.position, elementData[i].Element.PlayerRange, elementData[i].colliders);
//        for (int j = 0; j < elementData[i].colliders.Length; j++)
//        {
//            var objec = elementData[i].colliders[j];
//            if (objec)
//            {
//                var obj = objec.GetComponent<Interactable>();
//                if (obj)
//                {
//                    obj.ApplyElement(elementData[i].Element, gameObject.transform);
//                }
//            }
//        }
//        
        yield return new WaitForSeconds(10f);



    }
}