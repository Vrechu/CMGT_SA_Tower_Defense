using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float attackRange = 5;
    [SerializeField] private float attackRate = 1;
    [SerializeField] private float attackDamage = 50;
    private float attackTimer = 0;



    void Update()
    {
        Countdown();
        if (target == null || !IsInRange(target)) FindClosestTarget();
    }

    private void Countdown()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            if (target != null && IsInRange(target)) Shoot();
            else target = null;
        }
    }

    private bool IsInRange(Transform target)
    {
        if ((transform.position - target.position).magnitude < attackRange)
        {
            return true;
        }
        else return false;
    }

    void FindClosestTarget()
    {
        for (int i = 0; i < WaveManager.Instance.Wave.Count; i++)
        {
            if (WaveManager.Instance.Wave[i] != null && IsInRange(WaveManager.Instance.Wave[i].transform))
            {
                if (target == null) target = WaveManager.Instance.Wave[i].transform;
                else if ((transform.position - target.position).magnitude
                    < (transform.position - WaveManager.Instance.Wave[i].transform.position).magnitude)
                {
                    target = WaveManager.Instance.Wave[i].transform;
                }
            }
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
