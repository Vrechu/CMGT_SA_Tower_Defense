using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    private float[] costs;
    private TowerID selectedTower;

    private void OnEnable()
    {
        EventBus<TowerCostsChangedEvent>.Subscribe(SetCosts);
        EventBus<TowerSelectedEvent>.Subscribe(SetSelected);
    }

    private void OnDestroy()
    {
        EventBus<TowerCostsChangedEvent>.UnSubscribe(SetCosts);
        EventBus<TowerSelectedEvent>.UnSubscribe(SetSelected);
    }
    public void SelectTower(int type)
    {
        EventBus<TowerSelectedFromMenuEvent>.Publish(new TowerSelectedFromMenuEvent(costs[type], type));
    }

    private void SetCosts(TowerCostsChangedEvent towerCostsChangedEvent)
    {
        costs = towerCostsChangedEvent.costs;
    }

    public void Upgrade()
    {
        EventBus<UpgradeButtonPressedEvent>.Publish(new UpgradeButtonPressedEvent(selectedTower));
    }

    private void SetSelected(TowerSelectedEvent towerSelectedEvent)
    {
        selectedTower = towerSelectedEvent.towerID;
    }

    public void StartWave()
    {
        EventBus<StartPressedEvent>.Publish(new StartPressedEvent());
    }
}
