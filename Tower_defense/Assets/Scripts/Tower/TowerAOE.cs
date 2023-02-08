using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAOE : MonoBehaviour, ITowerAttack
{
    private float attackRange = 5;
    private float attackRate = 1;
    private float attackDamage = 50;
    private float attackTimer = 0;
    ITowerTargeting targetSystem;
    private TowerID ID;

    private void OnEnable()
    {
        ID = GetComponent<TowerID>();
        EventBus<TowerStatsChangedEvent>.Subscribe(OnTowerStatsChange);
    }

    private void OnDestroy()
    {
        EventBus<TowerStatsChangedEvent>.UnSubscribe(OnTowerStatsChange);
    }

    private void Start()
    {
        targetSystem = new ClosestInRangeTargeting(transform, attackRange);
    }

    public void Enabled(bool enable)
    {
        this.enabled = enable;
    }

    void Update()
    {
        Countdown();
    }

    private void Countdown()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            if (targetSystem.EnemiesInRange().Count > 0) Shoot();
        }
    }

    void Shoot()
    {
        if (targetSystem.EnemiesInRange() != null)
        {
            foreach (KeyValuePair<uint, Transform> enemy in targetSystem.EnemiesInRange())
            {
                EventBus<TowerAttackEvent>.Publish(new TowerAttackEvent(enemy.Value.GetComponent<EnemyID>().ID, attackDamage));
                attackTimer = attackRate;
            }
        }
    }
    private void OnTowerStatsChange(TowerStatsChangedEvent towerStatsChangedEvent)
    {
        attackRange = ID.attackRange;
        attackDamage = ID.attackDamage;
        attackRate = ID.attackCooldown;
    }

    private void OnDrawGizmos()
    {
        if (this.enabled && targetSystem.EnemiesInRange().Count > 0)
        {
            foreach (KeyValuePair<uint, Transform> enemy in targetSystem.EnemiesInRange())
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, enemy.Value.position);
            }
        }
    }

}
