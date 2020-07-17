using UnityEngine;

/// <summary>
/// Spawns particles around objects of interest 
/// </summary>
public class FireflyController : MonoBehaviour
{
    private ParticleSystem fireflies;
    private GameObject player;
    private ParticleSystem.EmissionModule fireflyRate;
    private ParticleSystem.MinMaxCurve orgRate;

    public bool interacted = false;
    
    public float disappearDistance = 7;
    
    void Start()
    {
        fireflies = GetComponentInChildren<ParticleSystem>();
        if (fireflies)
        {
            fireflyRate = fireflies.emission;
        }
        player = GameObject.FindWithTag("Player");
        orgRate = fireflyRate.rateOverTime;

        if (fireflies == null)
        {
            Debug.LogWarning("Fireflies not found " + gameObject.name);
        }
    }
    
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
