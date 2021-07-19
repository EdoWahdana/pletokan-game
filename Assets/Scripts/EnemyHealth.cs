using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [HideInInspector]
    public int currentHealth;

    public ParticleSystem enemyDestroy;
    public EnemyHealthBar enemyHealthBar;
    public int startingHealth;

    private NPCManage npcManage;
    private Transform player;
    private float distance;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        npcManage = GameObject.FindGameObjectWithTag("NPCManage").GetComponent<NPCManage>();
        enemyHealthBar.SetEnemyMaxHealth(startingHealth);
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        enemyHealthBar.SetEnemyHealth(currentHealth);
        if(currentHealth <= 0) {
            npcManage.DecreaseEnemy();
            enemyDestroy.Play();
            Destroy(transform.parent.gameObject);
        }
    }

    public bool EnemyCheck()
    {
        if (!gameObject)
            return false; 
        return true;
    }

    public float FleePosition()
    { 
        distance = Vector3.Distance(transform.parent.position, player.position);
        return distance;
    }

    public Vector3 FleeDestination()
    {
        Vector3 runTo = transform.parent.position + ((transform.parent.position - player.position) * 2);
        return runTo;
    }
}
