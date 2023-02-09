using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRegularAttack : MonoBehaviour, ITowerAttack
{
    public uint target;
    private float attackRange = 5;
    private float attackCooldown = 1;
    private float attackDamage = 50;
    ITowerTargeting targetSystem;
    private TowerID ID;
    private CountDownTimer attackCountdownTimer = new CountDownTimer(0, true);

    private void OnEnable()
    {
        ID = GetComponent<TowerID>();
        EventBus<TowerStatsChangedEvent>.Subscribe(OnTowerStatsChange);
        ChangeStats(ID);
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
        if (attackCountdownTimer.CountDown()) Shoot();
        target = targetSystem.Target();
    }

    void Shoot()
    {
        EventBus<TowerAttackEvent>.Publish(new TowerAttackEvent(target, attackDamage));
    }

    private void OnTowerStatsChange(TowerStatsChangedEvent towerStatsChangedEvent)
    {
        ChangeStats(towerStatsChangedEvent.towerID);
    }

    private void OnDrawGizmos()
    {
        if (this.enabled && targetSystem.EnemiesInRange().ContainsKey(target))
        {
            
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, targetSystem.EnemiesInRange()[target].position);
        }
    }

    private void ChangeStats(TowerID iD)
    {
        attackRange = ID.attackRange;
        attackDamage = ID.attackDamage;
        attackCooldown = ID.attackCooldown;
        attackCountdownTimer.SetCountdownTime(attackCooldown);
    }
}
