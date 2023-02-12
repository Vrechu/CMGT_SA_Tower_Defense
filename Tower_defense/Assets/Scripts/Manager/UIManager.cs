using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private float money;
    [SerializeField] private TextMeshProUGUI moneyUI;
    [SerializeField] private TextMeshProUGUI moneyGainedUI;

    [SerializeField] private TextMeshProUGUI lives;
    [SerializeField] private TextMeshProUGUI wave;

    private float[] towerCosts;
    [SerializeField] private TextMeshProUGUI[] towerCostsUI;

    private float upgradeCost;
    [SerializeField] private GameObject Upgrade;
    [SerializeField] private TextMeshProUGUI upgradeCostUI;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    private CountdownTimer moneyGainedUITimer = new CountdownTimer(0.5f, false);

    private void OnEnable()
    {
        EventBus<MoneyChangedEvent>.Subscribe(OnMoneyChanged);
        EventBus<LivesChangedEvent>.Subscribe(SetLivesUI);
        EventBus<WaveStartEvent>.Subscribe(SetWaveUI);
        EventBus<TowerCostsChangedEvent>.Subscribe(OnTowerCostsChanged);
        EventBus<TowerSelectedEvent>.Subscribe(OnTowerSelected);
        EventBus<DeselectEvent>.Subscribe(HideUpgradeButton);
        EventBus<TowerStatsChangedEvent>.Subscribe(OnUpgrade);
        EventBus<WinGameEvent>.Subscribe(OnGameWin);
        EventBus<LoseGameEvent>.Subscribe(OnGameLose);
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
        EventBus<WinGameEvent>.UnSubscribe(OnGameWin);
        EventBus<LoseGameEvent>.UnSubscribe(OnGameLose);
    }

    private void Start()
    {
        Upgrade.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    private void Update()
    {
        if (moneyGainedUITimer.CountDown()) moneyGainedUI.enabled = false;
    }

    private void OnMoneyChanged(MoneyChangedEvent moneyChangedEvent)
    {
        SetMoneyGainedUI(moneyChangedEvent.money);
        SetMoneyUI(moneyChangedEvent.money);
        SetUpgradeCostUI(upgradeCost);
        if (towerCosts != null) SetTowerCostsUI(towerCosts);
    }

    private void OnTowerCostsChanged(TowerCostsChangedEvent towerCostsChangedEvent)
    {
        SetTowerCostsUI(towerCostsChangedEvent.costs);
    }

    private void OnUpgrade(TowerStatsChangedEvent towerStatsChangedEvent)
    {
        SetUpgradeCostUI(towerStatsChangedEvent.towerID.currentUpgradeCost);
    }   

    private void OnGameWin(WinGameEvent winGameEvent)
    {
        winScreen.SetActive(true);
    }
    private void OnGameLose(LoseGameEvent loseGameEvent)
    {
        loseScreen.SetActive(true);
    }

    private void OnTowerSelected(TowerSelectedEvent towerSelectedEvent)
    {
        Upgrade.SetActive(true);
        SetUpgradeCostUI(towerSelectedEvent.towerID.currentUpgradeCost);
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

    private void SetLivesUI(LivesChangedEvent livesChangedEvent)
    {
        lives.SetText("Lives: " + livesChangedEvent.lives);
    }

    private void SetWaveUI(WaveStartEvent waveStartEvent)
    {
        wave.SetText("Wave: " + (waveStartEvent.waveNumber + 1));
    }

    private void SetUpgradeCostUI(float pCost)
    {
        upgradeCost = pCost;
        upgradeCostUI.SetText(pCost.ToString());
        if (upgradeCost <= money) upgradeCostUI.color = Color.green;
        else upgradeCostUI.color = Color.red;
    }
    private void HideUpgradeButton(DeselectEvent deselectEvent)
    {
        Upgrade.SetActive(false);
    }

    private void SetMoneyGainedUI(float newMoney)
    {
        float moneyGained = newMoney - money;
        string text = "+";
        moneyGainedUI.SetText(text + moneyGained.ToString());
        moneyGainedUI.color = Color.green;
        if (moneyGained < 0)
        {
            moneyGainedUI.color = Color.red;
            moneyGainedUI.SetText(moneyGained.ToString());
        }
        moneyGainedUI.enabled = true;
        moneyGainedUITimer.Reset();
    }
}

