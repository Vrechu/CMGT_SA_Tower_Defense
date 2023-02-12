using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSlider : MonoBehaviour
{
    private Slider healthSlider;
    private EnemyHealth enemyHealth;
    private bool maxValueSet = false;

    private void OnEnable()
    {
        healthSlider = GetComponent<Slider>();
        enemyHealth = GetComponentInParent<EnemyHealth>();

        enemyHealth.OnEnemyHealthChanged += SetHealthSlider;
    }

    private void OnDestroy()
    {
        enemyHealth.OnEnemyHealthChanged -= SetHealthSlider;
    }

    private void SetHealthSlider(float health)
    {
        if (!maxValueSet)
        {
            healthSlider.maxValue = health;
            maxValueSet = true;
        }
        healthSlider.value = health;
    }
}
