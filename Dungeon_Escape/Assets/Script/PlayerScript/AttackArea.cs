using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 1;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<Healths>() != null)
        {
            Healths health = collider.GetComponent<Healths>();
            health.Damage(damage);
        }
        
        if(collider.GetComponent<Health>() != null)
        {
            Health healths = collider.GetComponent<Health>();
            healths.Damage(damage);
        }
    }
}
