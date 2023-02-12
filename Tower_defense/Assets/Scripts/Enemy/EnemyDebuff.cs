using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebuff : MonoBehaviour
{
    private EnemyID ID;

    private CountdownTimer debuffTimer;

    private void OnEnable()
    {
        ID = GetComponent<EnemyID>();
        EventBus<TowerDebuffEvent>.Subscribe(Debuff);
        debuffTimer = new CountdownTimer(0, false);
    }

    private void OnDestroy()
    {
        EventBus<TowerDebuffEvent>.UnSubscribe(Debuff);
    }

    private void Update()
    {
        Debuffed();
    }

    public bool Debuffed()
    {
        if (debuffTimer.CountDown()) return false;
        return true;
    }

    public void Debuff(TowerDebuffEvent towerDebuffEvent)
    {
        if (!Debuffed() && towerDebuffEvent.enemyID == ID.ID)
        {
            debuffTimer.SetCountdownTime(towerDebuffEvent.time);
            debuffTimer.Reset();
        }
    }
}
