using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kail_R : MonoBehaviour
{
    ParticleSystem particle;
    public void Trigger()
    {
        particle = GetComponent<ParticleSystem>();
        StartCoroutine(R());
    }
    public IEnumerator R()
    {
        yield return new WaitForSeconds(particle.main.startLifetimeMultiplier);
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        
    }
}
