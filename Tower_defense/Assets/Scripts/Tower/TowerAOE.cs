using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAOE : MonoBehaviour, ITowerAttack
{
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
        if (attackCountdownTimer.CountDown() && targetSystem.EnemiesInRange().Count > 0) Shoot();
    }

    void Shoot()
    {
        if (targetSystem.EnemiesInRange() != null)
        {
            foreach (KeyValuePair<uint, Transform> enemy in targetSystem.EnemiesInRange())
            {
                EventBus<TowerAttackEvent>.Publish(new TowerAttackEvent(enemy.Value.GetComponent<EnemyID>().ID, attackDamage));
            }
        }
    }
    private void OnTowerStatsChange(TowerStatsChangedEvent towerStatsChangedEvent)
    {
        AddUpgradeStats(towerStatsChangedEvent.towerID);
    }

    private void SetStartingStats()
    {
        attackRange = ID.towerStats.StartingAttackRange;
        attackCooldown = ID.towerStats.StartingAttackCooldown;
        attackDamage = ID.towerStats.StartingAttackDamage;
        attackCountdownTimer.SetCountdownTime(attackCooldown);
    }

    private void AddUpgradeStats(TowerID iD)
    {
        attackRange += ID.towerStats.UpgradeRangeIncrease;
        attackDamage += ID.towerStats.UpgradeDamageIncrease;
        attackCooldown *= ID.towerStats.UpgradeCooldownMultiplier;
        attackCountdownTimer.SetCountdownTime(attackCooldown);
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
