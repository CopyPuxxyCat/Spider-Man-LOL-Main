using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auto_destroy_4s : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DisableEnemy());
    }

    private IEnumerator DisableEnemy()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
        
    }
}
