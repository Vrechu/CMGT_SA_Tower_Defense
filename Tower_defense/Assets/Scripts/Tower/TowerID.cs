using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerID : MonoBehaviour
{
    public uint ID;
    public float upgradeCost = 75;
    /*public float attackRange = 5;
    public float attackCooldown = 1;
    public float attackDamage = 50;
    public float debuffTime = 4;
    [SerializeField] private float upgradeCostIncrease = 50;
    [SerializeField] private float upgradeDamageIncrease = 2;
    [SerializeField] private float upgradeRangeIncrease = 2;
    [SerializeField] private float upgradeCooldownMultiplier = 0.8f;
    [SerializeField] private float upgradeDebuffTimeIncrease = 2;*/

    public TowerStats towerStats;

    public void SetID(uint cID)
    {
        ID = cID;
    }

    private void OnEnable()
    {
        EventBus<EnoughMoneyForUpgradeEvent>.Subscribe(Upgrade);
    }

    private void OnDestroy()
    {
        EventBus<EnoughMoneyForUpgradeEvent>.UnSubscribe(Upgrade);
    }

    public void Upgrade(EnoughMoneyForUpgradeEvent enoughMoneyForUpgradeEvent)
    {
        if (enoughMoneyForUpgradeEvent.towerID.ID == ID)
        {
            attackRange += upgradeRangeIncrease;
            attackDamage += upgradeDamageIncrease;
            attackCooldown *= upgradeCooldownMultiplier;
            debuffTime += upgradeDebuffTimeIncrease;
            upgradeCost += upgradeCostIncrease;
            EventBus<TowerStatsChangedEvent>.Publish(new TowerStatsChangedEvent(this));
        }
    }
}
