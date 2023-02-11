using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerTargeting
{
    uint Target();
    void SetAttackRange(float attackRange);
    void SetTransform(Transform pTransform);
    Dictionary<uint, Transform> EnemiesInRange();
}
