using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100;
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
            EventBus<EnemyKilledEvent>.Publish(new EnemyKilledEvent(moneyWorth));
            
            Destroy(gameObject);
        }
    }

}
