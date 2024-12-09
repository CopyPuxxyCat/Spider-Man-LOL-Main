using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theocamera : MonoBehaviour
{
    private Camera cam;
    
    private void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.forward = cam.transform.forward;
    }
}
