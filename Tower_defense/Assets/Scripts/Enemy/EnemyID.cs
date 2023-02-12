using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyID : MonoBehaviour
{
    public uint ID { get; private set; }
    public EnemyStats enemyStats;

    /*public float moneyWorth;
    public float health;
    public float speed;*/

    public void SetID(uint cID)
    {
        ID = cID;
    }
}
