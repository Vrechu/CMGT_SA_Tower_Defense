using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAOE : MonoBehaviour, ITowerAttack
{
    [SerializeField] private float attackRange = 5;
    [SerializeField] private float attackRate = 1;
    [SerializeField] private float attackDamage = 50;
    private float attackTimer = 0;
    ITowerTargeting targetSystem;

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
                enemy.Value.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
                attackTimer = attackRate;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (this.enabled && targetSystem.EnemiesInRange().Count > 0)
        {
            foreach (KeyValuePair<uint, Transform> enemy in targetSystem.EnemiesInRange())
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, enemy.Value.position);
            }
        }
    }
}
