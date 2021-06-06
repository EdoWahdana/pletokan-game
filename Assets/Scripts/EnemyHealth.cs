using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [HideInInspector]
    public int currentHealth;

    public EnemyHealthBar enemyHealthBar;
    public int startingHealth;
    
    void Start()
    {
        enemyHealthBar.SetEnemyMaxHealth(startingHealth);
        currentHealth = startingHealth;
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        enemyHealthBar.SetEnemyHealth(currentHealth);
        if(currentHealth <= 0)
            Die();
    }
}
