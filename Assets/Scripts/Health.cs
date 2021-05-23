using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [HideInInspector]
    public int startingHealth = 100;

    public HealthBar healthBar;
    public int currentHealth;
    
    void Start()
    {
        healthBar.SetMaxHealth(startingHealth);
        currentHealth = startingHealth;
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0)
            Die();
    }
}
