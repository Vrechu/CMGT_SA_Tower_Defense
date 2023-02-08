using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Event { }

public class EventBus<T> where T : Event
{
    public static event Action<T> OnEvent;

    public static void Subscribe(Action<T> method)
    {
        OnEvent += method;
    }

    public static void UnSubscribe(Action<T> method)
    {
        OnEvent -= method;
    }

    public static void Publish(T pEvent)
    {
        OnEvent?.Invoke(pEvent);
    }
}

#region ENEMIES
public class EnemySpawnedEvent: Event
{
    public uint ID;
    public EnemySpawnedEvent(uint cID)
    {
        ID = cID;
    }
}
public class EnemyKilledEvent : Event {
    public uint enemy;
    public float money;
    public EnemyKilledEvent(uint cEnemy, float cMoney) { enemy = cEnemy;  money = cMoney; }
}
public class EnemyReachedEndEvent : Event
{
    public uint enemy;
    public EnemyReachedEndEvent(uint cEnemy) { enemy = cEnemy;}
    
}
public class AllEnemiesGoneEvent : Event{ }
public class WaveChangedEvent : Event
{
    public Dictionary<uint, Transform> wave;
    public WaveChangedEvent(Dictionary<uint, Transform> cWave)
    {
        wave = cWave;
    }
}

#endregion


//EVENTS

#region TOWERS
public class TowerSelectedFromMenuEvent : Event 
{
    public float money;
    public int towerType;
    public TowerSelectedFromMenuEvent(float cMoney, int cTowerType)
    {
        money = cMoney;
        towerType = cTowerType;
    }
}
public class TowerAttackEvent : Event
{
    public uint enemyID;
    public float damage;
    public TowerAttackEvent(uint cEnemyID, float cDamage)
    {
        enemyID = cEnemyID;
        damage = cDamage;
    }
}
public class TowerDebuffEvent : Event
{
    public uint enemyID;
    public float time;
    public TowerDebuffEvent(uint cEnemyID, float cTime)
    {
        enemyID = cEnemyID;
        time = cTime;
    }
}
public class TowerPlacedEvent : Event
{
    public int type;
    public float cost;
    public TowerPlacedEvent(int cType, float cCost)
    {
        type = cType;
        cost = cCost;
    }
}
public class TowerCostsChangedEvent : Event
{
    public float[] costs;
    public TowerCostsChangedEvent(float[] cCosts)
    {
        costs = cCosts;
    }
}
public class TowerStatsChangedEvent : Event
{
    public TowerID towerID;
    public TowerStatsChangedEvent(TowerID cTowerID)
    {
        towerID = cTowerID;
    }
}
public class TowerSelectedEvent : Event
{
    public TowerID towerID;
    public TowerSelectedEvent(TowerID cTowerID)
    {
        towerID = cTowerID;
    }
}
public class UpgradeButtonPressedEvent : Event
{
    public TowerID towerID;
    public UpgradeButtonPressedEvent(TowerID cTowerID)
    {
        towerID = cTowerID;
    }
}

public class EnoughMoneyForUpgradeEvent : Event
{
    public TowerID towerID;
    public EnoughMoneyForUpgradeEvent(TowerID cTowerID)
    {
        towerID = cTowerID;
    }
}

#endregion



#region MONEY
public class MoneyChangedEvent : Event
{
    public MoneyChangedEvent(float cMoney) { money = cMoney; }
    public float money;
}
public class EnoughMoneyForTowerEvent : Event
{
    public int towerType;
    public EnoughMoneyForTowerEvent(int cTowerType)
    {
        towerType = cTowerType;
    }
}

#endregion

public class LivesChangedEvent : Event
{
    public LivesChangedEvent(float cLives) { lives = cLives; }
    public float lives;
}
public class WaveStartEvent: Event
{
    public int waveNumber;
    public int enemies;
    public WaveStartEvent(int cWavenumber, int cEnemies) { waveNumber = cWavenumber; enemies = cEnemies; }
}
public class BuildingFaseEndedEvent : Event { }
public class DeselectEvent : Event { }


