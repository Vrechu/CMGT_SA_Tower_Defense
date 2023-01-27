using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; set; }

    private float money = 300;

    private void Start()
    {
        EventBus<MoneyChangedEvent>.Publish(new MoneyChangedEvent(money));
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        EventBus<EnemyKilledEvent>.Subscribe(AddMoney);
        EventBus<TowerPlacedEvent>.Subscribe(RemoveMoney);
    }

    private void OnDestroy()
    {
        EventBus<EnemyKilledEvent>.UnSubscribe(AddMoney);
        EventBus<TowerPlacedEvent>.UnSubscribe(RemoveMoney);
    }

    public void AddMoney(EnemyKilledEvent enemyKilledEvent)
    {
        money += enemyKilledEvent.money;
        EventBus<MoneyChangedEvent>.Publish(new MoneyChangedEvent(money));
    }

    public void RemoveMoney(TowerPlacedEvent towerPlacedEvent)
    {
        money -= towerPlacedEvent.cost;
        if (money < 0) money = 0;
        EventBus<MoneyChangedEvent>.Publish(new MoneyChangedEvent(money));
    }

    public float GetMoney()
    {
        return money;
    }
}
