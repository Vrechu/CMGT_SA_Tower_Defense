using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public event Action<float> OnEnemyHealthChanged;
    private float health;
    private EnemyID ID;

    private void OnEnable()
    {
        ID = GetComponent<EnemyID>();
        health = ID.EnemyStats.MaxHealth;
        EventBus<TowerAttackEvent>.Subscribe(TakeDamage);
    }

    private void OnDestroy()
    {
        EventBus<TowerAttackEvent>.UnSubscribe(TakeDamage);
    }

    private void Start()
    {
        OnEnemyHealthChanged?.Invoke(health);
    }

    public void TakeDamage(TowerAttackEvent towerAttackEvent)
    {
        if (towerAttackEvent.enemyID == ID.ID)
        {
            if (GetComponent<EnemyDebuff>().Debuffed()) health -= towerAttackEvent.damage;
            health -= towerAttackEvent.damage;
            OnEnemyHealthChanged?.Invoke(health);
            CheckHealth();
        }
    }

    private void CheckHealth()
    {
        if(health <= 0)
        {
            EventBus<EnemyKilledEvent>.Publish(new EnemyKilledEvent(ID.ID,ID.EnemyStats.MoneyWorth));
            
            Destroy(gameObject);
        }
    }

}