using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [HideInInspector]
    public int currentHealth;

    public PlayerHealthBar playerHealthBar;
    public int startingHealth;

    // Use this for initialization
    void Start()
    {
        playerHealthBar.SetPlayerMaxHealth(startingHealth);
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        playerHealthBar.SetPlayerHealth(currentHealth);
        if (currentHealth <= 0) {
            //GameOver();
            //Game Over disini
        }
    }
}
