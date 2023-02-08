using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDebuff : MonoBehaviour, ITowerAttack
{
    public uint target;
    private float attackRange = 5;
    private float attackRate = 1;
    private float debuffTime = 3;
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
        target = targetSystem.Target();
    }

    private void Countdown()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            if (target != null) Shoot();
        }
    }

    void Shoot()
    {
        EventBus<TowerDebuffEvent>.Publish(new TowerDebuffEvent(target, debuffTime));
        attackTimer = attackRate;
    }

    private void OnTowerStatsChange(TowerStatsChangedEvent towerStatsChangedEvent)
    {
        attackRange = ID.attackRange;
        attackRate = ID.attackCooldown;
        attackRate = ID.debuffTime;
    }

    private void OnDrawGizmos()
    {
        if (this.enabled && targetSystem.EnemiesInRange().ContainsKey(target))
        {

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetSystem.EnemiesInRange()[target].position);
        }
    }
}
