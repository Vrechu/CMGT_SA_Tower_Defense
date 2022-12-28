using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI money;
    public TextMeshProUGUI lives;


    private void Start()
    {
        MoneyManager.Instance.OnMoneyChange += SetMoneyUI;
        FinishManager.Instance.OnLivesChange += SetLivesUI;
        SetLivesUI();
        SetMoneyUI();
    }

    private void OnDestroy()
    {
        MoneyManager.Instance.OnMoneyChange -= SetMoneyUI;
        FinishManager.Instance.OnLivesChange -= SetLivesUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetMoneyUI()
    {
        money.SetText("Money: " + MoneyManager.Instance.GetMoney());
    }

    private void SetLivesUI()
    {
        lives.SetText("Lives: " + FinishManager.Instance.lives);
    }
}
