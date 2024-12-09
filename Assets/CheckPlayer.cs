
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    private bool isPlayerInZone = false;
    public BossBehavior bb;

    private void Start()
    {
        //bb = GetComponent<BossBehavior>();
    }

    private void Update()
    {
        bb.isPlayerInAttackZone = isPlayerInZone;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            //bb.isPlayerInAttackZone = isPlayerInZone;
            Debug.Log("Player entered attack zone.");
        }
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            Debug.Log("Player entered attack zone.");
        }
    }*/

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            //bb.isPlayerInAttackZone = isPlayerInZone;
            Debug.Log("Player exited attack zone.");
        }
    }


}
