using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tests : MonoBehaviour
{
    public ParticleSystem a;
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<ParticleSystem>();
        StartCoroutine("test");
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(0.7f);
        ParticleSystem.ExternalForcesModule externalForcesModule = a.externalForces;
        externalForcesModule.influenceMask = LayerMask.GetMask("Ingredients");
    }
}
