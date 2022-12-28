using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100;
    public static event Action<float> OnEnemyDeath;
    public float moneyWorth = 50;
    

    public void TakeDamage( float damage)
    {
        CheckHealth();
        health -= damage;
    }

    void CheckHealth()
    {
        if(health <= 0)
        {
            OnEnemyDeath?.Invoke(moneyWorth);
            Destroy(gameObject);
        }
    }

}
