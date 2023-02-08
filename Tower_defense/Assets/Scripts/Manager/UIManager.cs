using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private float money;
    public TextMeshProUGUI moneyUI;
    public TextMeshProUGUI lives;
    public TextMeshProUGUI wave;

    private float[] towerCosts;
    public TextMeshProUGUI[] towerCostsUI;

    public GameObject Upgrade;
    public TextMeshProUGUI upgradeCostUI;
    private float upgradeCost;


    private void OnEnable()
    {
        EventBus<MoneyChangedEvent>.Subscribe(OnMoneyChanged);
        EventBus<LivesChangedEvent>.Subscribe(SetLivesUI);
        EventBus<WaveStartEvent>.Subscribe(SetWaveUI);
        EventBus<TowerCostsChangedEvent>.Subscribe(OnTowerCostsChanged);
        EventBus<TowerSelectedEvent>.Subscribe(OnTowerSelected);
        EventBus<DeselectEvent>.Subscribe(HideUpgradeButton);
        EventBus<TowerStatsChangedEvent>.Subscribe(OnUpgrade);
    }

    private void OnDestroy()
    {
        EventBus<MoneyChangedEvent>.UnSubscribe(OnMoneyChanged);
        EventBus<LivesChangedEvent>.UnSubscribe(SetLivesUI);
        EventBus<WaveStartEvent>.UnSubscribe(SetWaveUI);
        EventBus<TowerCostsChangedEvent>.UnSubscribe(OnTowerCostsChanged);
        EventBus<TowerSelectedEvent>.UnSubscribe(OnTowerSelected);
        EventBus<DeselectEvent>.UnSubscribe(HideUpgradeButton);
        EventBus<TowerStatsChangedEvent>.UnSubscribe(OnUpgrade);
    }

    private void Start()
    {
        Upgrade.SetActive(false);
    }

    private void OnMoneyChanged(MoneyChangedEvent moneyChangedEvent)
    {
        SetMoneyUI(moneyChangedEvent.money);
        SetUpgradeCostUI(upgradeCost);
        if (towerCosts != null) SetTowerCostsUI(towerCosts);
    }


    private void SetLivesUI(LivesChangedEvent livesChangedEvent)
    {
        lives.SetText("Lives: " + livesChangedEvent.lives);
    }

    private void SetWaveUI(WaveStartEvent waveStartEvent)
    {
        wave.SetText("Wave: " + waveStartEvent.waveNumber);
    }

    private void OnTowerCostsChanged(TowerCostsChangedEvent towerCostsChangedEvent)
    {
        SetTowerCostsUI(towerCostsChangedEvent.costs);
    }

    private void HideUpgradeButton(DeselectEvent deselectEvent)
    {
        Upgrade.SetActive(false);
    }

    private void OnTowerSelected(TowerSelectedEvent towerSelectedEvent)
    {
        Upgrade.SetActive(true);
        SetUpgradeCostUI(towerSelectedEvent.towerID.upgradeCost);
    }


    private void SetMoneyUI(float pMoney)
    {
        money = pMoney;
        moneyUI.SetText("Money: " + pMoney);
    }

    private void SetTowerCostsUI(float[] pCosts)
    {
        towerCosts = pCosts;
        for (int i = 0; i < towerCostsUI.Length; i++)
        {
            towerCostsUI[i].SetText("" + pCosts[i]);
            if (pCosts[i] <= money) towerCostsUI[i].color = Color.green;
            else towerCostsUI[i].color = Color.red;
        }
    }

    private void SetUpgradeCostUI(float pCost)
    {
        upgradeCost = pCost;
        upgradeCostUI.SetText(pCost.ToString());
        if (upgradeCost <= money) upgradeCostUI.color = Color.green;
        else upgradeCostUI.color = Color.red;
    }

    private void OnUpgrade(TowerStatsChangedEvent towerStatsChangedEvent)
    {
        SetUpgradeCostUI(towerStatsChangedEvent.towerID.upgradeCost);
    }
}

