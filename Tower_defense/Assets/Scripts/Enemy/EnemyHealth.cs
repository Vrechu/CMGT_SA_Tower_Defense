using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    private EnemyID ID;

    private void OnEnable()
    {
        ID = GetComponent<EnemyID>();
        health = ID.health;
    }

    public void TakeDamage( float damage)
    {
        if (GetComponent<EnemyDebuff>().Debuffed()) health -= damage;
        health -= damage;
        CheckHealth();
    }

    void CheckHealth()
    {
        if(health <= 0)
        {
            EventBus<EnemyKilledEvent>.Publish(new EnemyKilledEvent(ID.ID,ID.moneyWorth));
            
            Destroy(gameObject);
        }
    }

}