using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform basePosition;
    [SerializeField] private float speed = 3;

    public void Move()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        basePosition = GameObject.FindGameObjectWithTag("Finish").transform;
        agent.speed = speed;
        agent.SetDestination(basePosition.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
