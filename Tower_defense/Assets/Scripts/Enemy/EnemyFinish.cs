using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinish : MonoBehaviour
{
    private EnemyID ID;

    private void OnEnable()
    {
        ID = GetComponent<EnemyID>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            EventBus<EnemyReachedEndEvent>.Publish(new EnemyReachedEndEvent(ID.ID));
            Destroy(gameObject);
        }
    }
}
