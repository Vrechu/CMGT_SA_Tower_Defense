using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRegularAttack : MonoBehaviour, ITowerAttack
{
    private uint target;
    private float attackRange = 5;
    private float attackCooldown = 1;
    private float attackDamage = 50;
    private ITowerTargeting targetSystem;
    private TowerID ID;
    private CountdownTimer attackCountdownTimer = new CountdownTimer(0, true);

    private void OnEnable()
    {
        ID = GetComponent<TowerID>();
        EventBus<TowerStatsChangedEvent>.Subscribe(OnTowerStatsChange);
        SetStartingStats();
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
        if (attackCountdownTimer.CountDown() && targetSystem.EnemiesInRange().Count > 0 ) Shoot();
        target = targetSystem.Target();
    }

    void Shoot()
    {
        EventBus<TowerAttackEvent>.Publish(new TowerAttackEvent(target, attackDamage));
    }

    private void OnTowerStatsChange(TowerStatsChangedEvent towerStatsChangedEvent)
    {
        AddUpgradeStats(towerStatsChangedEvent.towerID);
    }

    private void OnDrawGizmos()
    {
        if (this.enabled && targetSystem.EnemiesInRange().ContainsKey(target))
        {
            
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, targetSystem.EnemiesInRange()[target].position);
        }
    }

    private void SetStartingStats()
    {
        attackRange = ID.towerStats.startingAttackRange;
        attackCooldown = ID.towerStats.startingAttackCooldown;
        attackDamage = ID.towerStats.startingAttackDamage;
        attackCountdownTimer.SetCountdownTime(attackCooldown);
    }

    private void AddUpgradeStats(TowerID iD)
    {
        attackRange += ID.towerStats.upgradeRangeIncrease;
        attackDamage += ID.towerStats.upgradeDamageIncrease;
        attackCooldown *= ID.towerStats.upgradeCooldownMultiplier;
        attackCountdownTimer.SetCountdownTime(attackCooldown);
    }
}
