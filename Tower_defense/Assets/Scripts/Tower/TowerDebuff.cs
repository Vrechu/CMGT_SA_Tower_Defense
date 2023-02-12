using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDebuff : MonoBehaviour, ITowerAttack
{
    private uint target;
    private float attackRange = 5;
    private float attackCooldown = 1;
    private float debuffTime = 3;
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
        target = targetSystem.Target();
    }

    void Shoot()
    {
        if (attackCountdownTimer.CountDown()) Shoot();
        EventBus<TowerDebuffEvent>.Publish(new TowerDebuffEvent(target, debuffTime));
    }

    private void OnTowerStatsChange(TowerStatsChangedEvent towerStatsChangedEvent)
    {
        AddUpgradeStats(towerStatsChangedEvent.towerID);
    }

    private void SetStartingStats()
    {
        attackRange = ID.towerStats.startingAttackRange;
        attackCooldown = ID.towerStats.startingAttackCooldown;
        debuffTime = ID.towerStats.startingDebuffTime;
        attackCountdownTimer.SetCountdownTime(attackCooldown);
    }

    private void AddUpgradeStats(TowerID iD)
    {
        attackRange += ID.towerStats.upgradeRangeIncrease;
        debuffTime += ID.towerStats.upgradeDebuffTimeIncrease;
        attackCooldown *= ID.towerStats.upgradeCooldownMultiplier;
        attackCountdownTimer.SetCountdownTime(attackCooldown);
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
