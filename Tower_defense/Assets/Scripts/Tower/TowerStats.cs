using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerStatsScriptableObject", menuName = "ScriptableObjects/TowerStats")]

public class TowerStats : ScriptableObject
{
    public float startingUpgradeCost = 75;
    public float upgradeCostIncrease = 50;
    public float startingAttackRange = 5;
    public float upgradeRangeIncrease = 2;
    public float startingAttackCooldown = 1;
    public float upgradeCooldownMultiplier = 0.8f;
    public float startingAttackDamage = 50;
    public float upgradeDamageIncrease = 2;
    public float startingDebuffTime = 4;
    public float upgradeDebuffTimeIncrease = 2;
}
