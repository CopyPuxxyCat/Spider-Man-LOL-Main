using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playVFX : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem particleSystem_1;
    public ParticleSystem particleSystem_2;
    public ParticleSystem particleSystem_3;
    public ParticleSystem particleSystem_4;// Kéo thả Particle System vào đây trong Inspector

    void Start()
    {
        //Debug.Log("goi play partical");
        PlayParticles();
    }

    private void Update()
    {
        //Debug.Log("goi play partical");
        PlayParticles();
    }

    void PlayParticles()
    {
        particleSystem_1.Play();
        particleSystem_2.Play();
        particleSystem_3.Play();
        particleSystem_4.Play();
    }
}
