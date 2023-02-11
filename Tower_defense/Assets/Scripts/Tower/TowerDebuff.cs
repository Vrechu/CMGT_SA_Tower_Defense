using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDebuff : MonoBehaviour, ITowerAttack
{
    public uint target;
    private float attackRange = 5;
    private float attackRate = 1;
    private float debuffTime = 3;
    ITowerTargeting targetSystem;
    private TowerID ID;
    private CountdownTimer attackCountdownTimer = new CountdownTimer(0, true);

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
        target = targetSystem.Target();
    }

    void Shoot()
    {
        if (attackCountdownTimer.CountDown()) Shoot();
        EventBus<TowerDebuffEvent>.Publish(new TowerDebuffEvent(target, debuffTime));
    }

    private void OnTowerStatsChange(TowerStatsChangedEvent towerStatsChangedEvent)
    {
        ChangeStats(towerStatsChangedEvent.towerID);
    }

    private void ChangeStats(TowerID iD)
    {
        attackRange = ID.attackRange;
        attackRate = ID.attackCooldown;
        attackRate = ID.debuffTime;
        attackCountdownTimer.SetCountdownTime(attackRate);
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
