using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyController : MonoBehaviour
{
    private ParticleSystem fireflies;
    private GameObject player;
    private ParticleSystem.EmissionModule fireflyRate;
    private ParticleSystem.MinMaxCurve orgRate;

    public bool interacted = false;
    
    public float disappearDistance = 7;

    // Start is called before the first frame update
    void Start()
    {
        fireflies = GetComponentInChildren<ParticleSystem>();
        fireflyRate = fireflies.emission;
        player = GameObject.FindWithTag("Player");
        orgRate = fireflyRate.rateOverTime;

        if (fireflies == null)
        {
            Debug.LogError("Fireflies not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fireflies != null)
        {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) < disappearDistance)
            {
                //Debug.Log(Vector3.Distance(gameObject.transform.position, player.transform.position));
                Disappear();
            }
            else
            {
                Reappear();
            }

            if (interacted)
            {
                Disappear();
            }
        }
        else
        {
            //Debug.LogError("Fireflies not found");
        }
    }

    void Disappear()
    {
        fireflyRate.rateOverTime = 0;
    }

    void Reappear()
    {
        if (!interacted)
        {
            fireflyRate.rateOverTime = orgRate;
        }
    }
}
