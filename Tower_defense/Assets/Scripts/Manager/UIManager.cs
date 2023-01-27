using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI money;
    public TextMeshProUGUI lives;


    private void OnEnable()
    {
        EventBus<MoneyChangedEvent>.Subscribe(SetMoneyUI);
        EventBus<LivesChangedEvent>.Subscribe(SetLivesUI);
    }

    private void OnDestroy()
    {
        EventBus<MoneyChangedEvent>.UnSubscribe(SetMoneyUI);
        EventBus<LivesChangedEvent>.UnSubscribe(SetLivesUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetMoneyUI(MoneyChangedEvent moneyChangedEvent)
    {
        money.SetText("Money: " + moneyChangedEvent.money);
    }

    private void SetLivesUI(LivesChangedEvent livesChangedEvent)
    {
        lives.SetText("Lives: " + livesChangedEvent.lives);
    }
}
