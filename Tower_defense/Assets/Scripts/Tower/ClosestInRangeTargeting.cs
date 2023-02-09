using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestInRangeTargeting: ITowerTargeting
{
    private float attackRange = 5;
    private Transform transform;
    private Dictionary<uint, Transform> wave = new Dictionary<uint, Transform>();
    private uint currentTarget;

    public ClosestInRangeTargeting(Transform cTransform, float cAttackRange)
    {
        transform = cTransform;
        attackRange = cAttackRange;

        EventBus<WaveChangedEvent>.Subscribe(UpdateWave);
    }

    ~ClosestInRangeTargeting()
    {
        EventBus<WaveChangedEvent>.UnSubscribe(UpdateWave);
    }

    public uint Target()
    {
        if (EnemiesInRange().ContainsKey(currentTarget)) return currentTarget;
        foreach (KeyValuePair<uint, Transform> target in EnemiesInRange())
        {
            if (!EnemiesInRange().ContainsKey(currentTarget)
                || (transform.position - EnemiesInRange()[currentTarget].position).magnitude
                < (transform.position - target.Value.position).magnitude)
            {
                currentTarget = target.Key;
            }
        }
        return currentTarget;
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
        foreach (KeyValuePair<uint,Transform> enemy in wave)
        {
            if (IsInRange(enemy.Value))
            {
                enemiesInRange.Add(enemy.Key, enemy.Value);
            }
        }
        return enemiesInRange;
    }

    private void UpdateWave(WaveChangedEvent waveChangedEvent)
    {
        wave = waveChangedEvent.wave;
    }

    
}
