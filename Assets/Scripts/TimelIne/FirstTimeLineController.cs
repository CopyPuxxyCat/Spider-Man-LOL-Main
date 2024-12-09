using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FirstTimeLineController : MonoBehaviour
{
    //public PlayableDirector playableDirectors_1;
    public PlayableDirector playableDirectors_2;
    //public PlayableDirector playableDirectors_3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Play();
        }
    }

    public void Play()
    {
        playableDirectors_2.Play();
    }

}
