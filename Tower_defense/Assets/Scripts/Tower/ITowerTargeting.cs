using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerTargeting
{
    Transform Target();
    void SetAttackRange(float attackRange);
    void SetTransform(Transform cTransform);
    Dictionary<uint, Transform> EnemiesInRange();
}
