using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [HideInInspector]
    public int currentHealth;

    public int startingHealth = 100;
    void Start()
    {
        currentHealth = startingHealth;
    }

    void Update()
    {
        
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
            Die();
    }
}
