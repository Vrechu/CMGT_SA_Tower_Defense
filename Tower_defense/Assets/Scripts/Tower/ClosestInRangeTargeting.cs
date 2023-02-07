using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestInRangeTargeting: ITowerTargeting
{
    private float attackRange = 5;
    private Transform transform;

    public ClosestInRangeTargeting(Transform cTransform, float cAttackRange)
    {
        transform = cTransform;
        attackRange = cAttackRange;

    }

    public Transform Target()
    {
        Transform newTarget = null;
        foreach (KeyValuePair<uint, Transform> target in EnemiesInRange())
        {
            if (newTarget == null) newTarget = target.Value;
            else if ((transform.position - newTarget.position).magnitude
                < (transform.position - target.Value.position).magnitude)
            {
                newTarget = target.Value;
            }
        }
        return newTarget;
    }


    public void SetAttackRange(float newAttackRange)
    {
        attackRange = newAttackRange;
    }

    public void SetTransform(Transform cTransform)
    {
        transform = cTransform;
    }


    private bool IsInRange(Transform target)
    {
        if (target != null && (transform.position - target.position).magnitude < attackRange)
        {
            return true;
        }
        else return false;
    }

    public Dictionary<uint, Transform> EnemiesInRange()
    {
        Dictionary<uint, Transform> enemiesInRange = new Dictionary<uint, Transform>();
        foreach (KeyValuePair<uint,Transform> enemy in  WaveManager.Instance.Wave)
        {
            if (IsInRange(enemy.Value))
            {
                enemiesInRange.Add(enemy.Key, enemy.Value);
            }
        }
        return enemiesInRange;
    }

    
}
