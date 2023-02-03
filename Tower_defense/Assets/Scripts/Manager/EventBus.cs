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

public class EnemyKilledEvent : Event {
    public EnemyKilledEvent(float cMoney) { money = cMoney; }
    public float money;
}
public class TowerPlacedEvent : Event
{
    public TowerPlacedEvent(float cCost) { cost = cCost; }
    public float cost;
}
public class MoneyChangedEvent : Event
{
    public MoneyChangedEvent(float cMoney) { money = cMoney; }
    public float money;
}
public class EnemyReachedEndEvent : Event { }
public class LivesChangedEvent : Event
{
    public LivesChangedEvent(float cLives) { lives = cLives; }
    public float lives;
}
public class WaveStartEvent: Event
{
    public WaveStartEvent(int cEnemies) { enemies = cEnemies; }
    public int enemies;
}
public class BuildingFaseEndedEvent : Event { }
public class AllEnemiesGoneEvent : Event{ }

