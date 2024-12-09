using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class testdamage : MonoBehaviour
{
    public AttributesManager nhanvat_atm;
    public AttributesManager enemy_atm;

    public Collider hitBox;

    private void Start()
    {
        
    }

    public void Enable_Collider()
    {
        hitBox.enabled = true;
        Debug.Log("ra don danh " + hitBox.enabled);
    }

    public void Disable_Collider()
    {
        hitBox.enabled = false;
        Debug.Log("het don danh");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            nhanvat_atm.dealdamage(enemy_atm.gameObject);
        if (Input.GetKeyDown(KeyCode.P))
            enemy_atm.dealdamage(nhanvat_atm.gameObject);
    }
}
