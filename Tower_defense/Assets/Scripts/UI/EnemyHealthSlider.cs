using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSlider : MonoBehaviour
{
    private Slider healthSlider;
    private EnemyHealth enemyHealth;
    void Start()
    {
        healthSlider = GetComponent<Slider>();
        enemyHealth = GetComponentInParent<EnemyHealth>();

        healthSlider.maxValue = enemyHealth.health;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthSlider.value = enemyHealth.health;
    }
}
