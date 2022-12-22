using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage( float damage)
    {
        CheckHealth();
        health -= damage;
    }

    void CheckHealth()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
