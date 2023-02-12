using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerID : MonoBehaviour
{
    public uint ID { get; private set; }
    
    public float currentUpgradeCost { get; private set; }

    public TowerStats towerStats;

    public void SetID(uint cID)
    {
        ID = cID;
    }

    private void OnEnable()
    {
        EventBus<EnoughMoneyForUpgradeEvent>.Subscribe(Upgrade);
        currentUpgradeCost = towerStats.startingUpgradeCost;
    }

    private void OnDestroy()
    {
        EventBus<EnoughMoneyForUpgradeEvent>.UnSubscribe(Upgrade);
    }

    public void Upgrade(EnoughMoneyForUpgradeEvent enoughMoneyForUpgradeEvent)
    {
        if (enoughMoneyForUpgradeEvent.towerID.ID == ID)
        {
            currentUpgradeCost += towerStats.upgradeCostIncrease;
            EventBus<TowerStatsChangedEvent>.Publish(new TowerStatsChangedEvent(this));
        }
    }
}
