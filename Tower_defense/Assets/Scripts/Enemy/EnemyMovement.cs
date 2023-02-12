using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform basePosition;
    private EnemyID ID;

    private void OnEnable()
    {
        ID = GetComponent<EnemyID>();
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        basePosition = GameObject.FindGameObjectWithTag("Finish").transform;
        agent.speed = ID.EnemyStats.Speed;
        agent.SetDestination(basePosition.position);
    }

    private void Update()
    {
        if (GetComponent<EnemyDebuff>().Debuffed()) agent.speed = ID.EnemyStats.Speed/ 2;
        else agent.speed = ID.EnemyStats.Speed;
    }
}
