using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public BoxCollider colliderPunch;
    public BoxCollider colliderKick;

    private void Start()
    {
        colliderPunch.enabled = false;
        colliderKick.enabled = false;
    }

    public void EnablePunch()
    {
        //Debug.Log("mo box");
        colliderPunch.enabled = true;
    }

    public void DisablePunch()
    {
        colliderPunch.enabled = false;
    }

    public void EnableKick()
    {
        colliderKick.enabled = true;
    }

    public void DisableKick()
    {
        colliderKick.enabled = false;
    }
}
