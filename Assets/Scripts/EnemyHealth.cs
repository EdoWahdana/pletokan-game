using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    [HideInInspector]
    public int currentHealth;

    public ParticleSystem enemyDestroy;
    int startingHealth = 100;

    private NPCManage npcManage;
    private Transform player;
    private float distance;

    public GameObject canvasNPC;
    private float waitTimeCanvasNPC = 1f;
    private bool showCanvas;

    public Slider slider;
    
    void Start()
    {
        slider = slider.GetComponent<Slider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        npcManage = GameObject.FindGameObjectWithTag("NPCManage").GetComponent<NPCManage>();
        currentHealth = startingHealth;
    }

    void Update()
    {
        if (showCanvas){
            waitTimeCanvasNPC -= Time.deltaTime;
            canvasNPC.SetActive(true);
            if(waitTimeCanvasNPC <= 0f){
                canvasNPC.SetActive(false);
                waitTimeCanvasNPC = 1f;
                showCanvas = false;
            }
        }   
    }

    public void TakeDamage(int damage)
    {
        showCanvas = true;
        currentHealth -= damage;
        enemyDestroy.Play();
        SetEnemyHealth(currentHealth);
        if(currentHealth <= 0) {
            npcManage.DecreaseEnemy();
            Destroy(transform.parent.gameObject);
        }
    }

    void SetEnemyHealth(int health)
    {
        slider.value = health;
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
