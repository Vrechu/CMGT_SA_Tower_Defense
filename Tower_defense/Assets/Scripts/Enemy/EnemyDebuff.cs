using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebuff : MonoBehaviour
{
    private EnemyID ID;

    [SerializeField] private float debuffTimer;

    private void OnEnable()
    {
        ID = GetComponent<EnemyID>();
    }
    private void Update()
    {
        Debuffed();
    }

    public bool Debuffed()
    {
        if (debuffTimer > 0)
        {
            debuffTimer -= Time.deltaTime;
            return true;
        }
        return false;
    }

    public void Debuff(float time)
    {
        if (!Debuffed()) debuffTimer = time;
    }
}
