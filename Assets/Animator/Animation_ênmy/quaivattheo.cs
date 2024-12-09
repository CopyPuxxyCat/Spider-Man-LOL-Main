using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quaivattheo : MonoBehaviour
{
    public float MaxSpeed;
    private float speed;

    private Collider[] hitCollider;
    private RaycastHit Hit;

    public float SightRange;
    public float DetectionRange;

    public Rigidbody rb;
    public GameObject Target;

    private bool seePlayer;
    void Start()
    {
        speed = MaxSpeed;
    }


    void Update()
    {
        if (!seePlayer)
        {
            hitCollider = Physics.OverlapSphere(transform.position, DetectionRange);
            foreach (var HitCollider in hitCollider)
            {
                if (HitCollider.tag == "Player")
                {
                    Target = HitCollider.gameObject;
                    seePlayer = true;
                }
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, (Target.transform.position - transform.position), out Hit, SightRange))
            {
                if (Hit.collider.tag != "Player")
                {
                    seePlayer = false;
                }
                else
                {
                    var Heading = Hit.transform.position - transform.position;
                    var Distance = Heading.sqrMagnitude;
                    var Direcation = Heading / Distance;

                    Vector3 Move = new Vector3(Direcation.x * speed, 0, Direcation.z * speed);
                    rb.velocity = Move;
                    transform.forward = Move;
                }
            }
        }
    }

    internal void TakeDamage(int damageAmount)
    {
        throw new NotImplementedException();
    }
}
