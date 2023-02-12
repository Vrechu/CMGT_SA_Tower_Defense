using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoneyManager : MonoBehaviour
{
    private float money = 300;
    
    private void OnEnable()
    {
        EventBus<EnemyKilledEvent>.Subscribe(AddMoney);

        EventBus<TowerPlacedEvent>.Subscribe(OnTowerPlaced);
        EventBus<TowerSelectedFromMenuEvent>.Subscribe(CheckIfEnoughMoney);
        EventBus<UpgradeButtonPressedEvent>.Subscribe(OnTowerUpgradePressed);
    }
    private void OnDestroy()
    {
        EventBus<EnemyKilledEvent>.UnSubscribe(AddMoney);

        EventBus<TowerPlacedEvent>.UnSubscribe(OnTowerPlaced);
        EventBus<TowerSelectedFromMenuEvent>.UnSubscribe(CheckIfEnoughMoney);
        EventBus<UpgradeButtonPressedEvent>.UnSubscribe(OnTowerUpgradePressed);
    }

    private void Start()
    {
        EventBus<MoneyChangedEvent>.Publish(new MoneyChangedEvent(money));        
    }

    private void AddMoney(EnemyKilledEvent enemyKilledEvent)
    {
        money += enemyKilledEvent.money;
        EventBus<MoneyChangedEvent>.Publish(new MoneyChangedEvent(money));
    }

    private void RemoveMoney(float pMoney)
    {
        money -= pMoney;
        if (money < 0) money = 0;
        EventBus<MoneyChangedEvent>.Publish(new MoneyChangedEvent(money));
    }

    private void OnTowerPlaced(TowerPlacedEvent towerPlacedEvent)
    {
        RemoveMoney(towerPlacedEvent.cost);
    }

    public float GetMoney()
    {
        return money;
    }

    private void CheckIfEnoughMoney(TowerSelectedFromMenuEvent towerSelectedFromMenuEvent)
    {
        if (towerSelectedFromMenuEvent.money <= money)
        {
            EventBus<EnoughMoneyForTowerEvent>.Publish(new EnoughMoneyForTowerEvent(towerSelectedFromMenuEvent.towerType));
        }
    }

    private void OnTowerUpgradePressed(UpgradeButtonPressedEvent upgradeButtonPressedEvent)
    {
        if (upgradeButtonPressedEvent.towerID.currentUpgradeCost <= money)
        {
            RemoveMoney(upgradeButtonPressedEvent.towerID.currentUpgradeCost);
            EventBus<EnoughMoneyForUpgradeEvent>.Publish(new EnoughMoneyForUpgradeEvent(upgradeButtonPressedEvent.towerID));
        }
    }
}
