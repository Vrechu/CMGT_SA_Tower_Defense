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
        EventBus<TowerAttackEvent>.Subscribe(TakeDamage);
    }

    private void OnDestroy()
    {
        EventBus<TowerAttackEvent>.UnSubscribe(TakeDamage);
    }

    public void TakeDamage(TowerAttackEvent towerAttackEvent)
    {
        if (towerAttackEvent.enemyID == ID.ID)
        {
            if (GetComponent<EnemyDebuff>().Debuffed()) health -= towerAttackEvent.damage;
            health -= towerAttackEvent.damage;
            CheckHealth();
        }
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