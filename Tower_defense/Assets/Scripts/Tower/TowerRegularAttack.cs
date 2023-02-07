using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRegularAttack : MonoBehaviour, ITowerAttack
{
    public Transform target;
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
        if (targetSystem.Target() != null) target = targetSystem.Target();
        else target = null;
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
        target.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        attackTimer = attackRate;
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
