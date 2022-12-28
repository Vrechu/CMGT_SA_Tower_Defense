using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; set; }

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
    }

    public void RemoveMoney(float amount)
    {
    
    }

    public float GetMoney()
    {
        return money;
    }
}
