using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cuop_event : MonoBehaviour
{
    public BoxCollider colliderCuop;

    private void Start()
    {
        colliderCuop.enabled = false;
    }

    public void EnableHitBox()
    {
        colliderCuop.enabled = true;
    }

    public void DisableHitBox()
    {
        colliderCuop.enabled = false;
    }
}
