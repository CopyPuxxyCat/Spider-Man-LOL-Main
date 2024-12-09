
using UnityEngine;

public class cuop_deal_dmg : MonoBehaviour
{
    public AttributesManager atm;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<AttributesManager>().TakeDamage(atm.tancong);
        }
    }
}
