using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyID : MonoBehaviour
{
    public uint ID { get; private set; }
    public EnemyStats EnemyStats;

    public void SetID(uint PID)
    {
        ID = PID;
    }
}
