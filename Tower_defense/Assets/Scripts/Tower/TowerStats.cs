using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerStatsScriptableObject", menuName = "ScriptableObjects/TowerStats")]

public class TowerStats : ScriptableObject
{
    public float StartingUpgradeCost = 75;
    public float UpgradeCostIncrease = 50;
    public float StartingAttackRange = 5;
    public float UpgradeRangeIncrease = 2;
    public float StartingAttackCooldown = 1;
    public float UpgradeCooldownMultiplier = 0.8f;
    public float StartingAttackDamage = 50;
    public float UpgradeDamageIncrease = 2;
    public float StartingDebuffTime = 4;
    public float UpgradeDebuffTimeIncrease = 2;
}
