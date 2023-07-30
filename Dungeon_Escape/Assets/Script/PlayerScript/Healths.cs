using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healths : MonoBehaviour
{
    [SerializeField] private AudioSource EnemyDeath;
    [SerializeField] private int health = 1;

    public void Damage(int amount)
    {
        this.health -= amount;

        if(health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        EnemyDeath.Play();
        Destroy(gameObject);
    }
}
