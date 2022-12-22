using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinish : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            FinishManager.Instance.ReduceLives();
            Destroy(gameObject);
        }
    }
}
