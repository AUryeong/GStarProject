using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QusetParticle : MonoBehaviour
{
    private ParticleSystem particle;
    void Start() 
    {
        particle = GetComponent<ParticleSystem>();
    }


}
