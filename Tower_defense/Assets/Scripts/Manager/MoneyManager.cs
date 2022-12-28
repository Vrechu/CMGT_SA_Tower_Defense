using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; set; }

    public event Action OnMoneyChange;

    private float money = 300;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        EnemyHealth.OnEnemyDeath += AddMoney;
        PlaceTower.OnTowerPlaced += RemoveMoney;
    }

    private void OnDestroy()
    {
        EnemyHealth.OnEnemyDeath -= AddMoney;
        PlaceTower.OnTowerPlaced -= RemoveMoney;
    }

    public void AddMoney(float amount)
    {
        money += amount;
        OnMoneyChange?.Invoke();
    }

    public void RemoveMoney(float amount)
    {
        money -= amount;
        if (money < 0) money = 0;
        OnMoneyChange?.Invoke();
    }

    public float GetMoney()
    {
        return money;
    }
}
