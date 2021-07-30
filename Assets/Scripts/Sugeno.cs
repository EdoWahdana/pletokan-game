using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sugeno : MonoBehaviour
{
    //public PlayerHealth playerHealth;
    private PlayerHealth playerHealth;
    private GameManage gameManage;
    private NPCManage npcManage;

    private bool skorBanyak, skorSedang, skorSedikit;
    private bool healthBanyak, healthSedang, healthSedikit;
    private bool jarakJauh, jarakSedang, jarakDekat;
    public int skor, health;
    public float jarak;
    public string rule;

    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        gameManage = GameObject.FindGameObjectWithTag("GameManage").GetComponent<GameManage>();
        npcManage = GameObject.FindGameObjectWithTag("NPCManage").GetComponent<NPCManage>();
    }

    void Update()
    {
        rule = Logic();
        skor = gameManage.playerScore;
        health = playerHealth.currentHealth;
        jarak = Vector3.Distance(transform.position, playerHealth.transform.position);

        //Inisiasi range skor
        skorSedikit = (skor >= 0 && skor <= 50) ? true : false;
        skorSedang = (skor >= 20 && skor <= 80) ? true : false;
        skorBanyak = (skor >= 50 && skor <= 100) ? true : false;

        //Inisiasi range health
        healthSedikit = (health >= 0 && health <= 45) ? true : false;
        healthSedang = (health >= 35 && health <= 70) ? true : false;
        healthBanyak = (health >= 60 && health <= 100) ? true : false;

        //Inisiasi range jarak Note : udah diedit angkanya
        jarakDekat = (jarak >= 0 && jarak <= 16) ? true : false;
        jarakSedang = (jarak >= 16 && jarak <= 23) ? true : false;
        jarakJauh = (jarak >= 24 && jarak <= 32) ? true : false;
    }

    public string Logic()
    {
        if (skorBanyak && healthBanyak && jarakJauh)
            return "Diam";
        else if (skorBanyak && healthBanyak && jarakSedang)
            return "Kabur";
        else if (skorBanyak && healthBanyak && jarakDekat)
            return "Kabur";
        else if (skorBanyak && healthSedang && jarakJauh)
            return "Diam";
        else if (skorBanyak && healthSedang && jarakSedang)
            return "Menyerang";
        else if (skorBanyak && healthSedang && jarakDekat)
            return "Menyerang";
        else if (skorBanyak && healthSedikit && jarakJauh)
            return "Diam";
        else if (skorBanyak && healthSedikit && jarakSedang)
            return "Menyerang";
        else if (skorBanyak && healthSedikit && jarakDekat)
            return "Menyerang";
        else if (skorSedang && healthBanyak && jarakJauh)
            return "Diam";
        else if (skorSedang && healthBanyak && jarakSedang)
            return "Menyerang";
        else if (skorSedang && healthBanyak && jarakDekat)
            return "Kabur";
        else if (skorSedang && healthSedang && jarakJauh)
            return "Diam";
        else if (skorSedang && healthSedang && jarakSedang)
            return "Kabur";
        else if (skorSedang && healthSedang && jarakDekat)
            return "Menyerang";
        else if (skorSedang && healthSedikit && jarakJauh)
            return "Menyerang";
        else if (skorSedang && healthSedikit && jarakSedang)
            return "Menyerang";
        else if (skorSedang && healthSedikit && jarakDekat)
            return "Kabur";
        else if (skorSedikit && healthBanyak && jarakJauh)
            return "Menyerang";
        else if (skorSedikit && healthBanyak && jarakSedang)
            return "Menyerang";
        else if (skorSedikit && healthBanyak && jarakDekat)
            return "Kabur";
        else if (skorSedikit && healthSedang && jarakJauh)
            return "Diam";
        else if (skorSedikit && healthSedang && jarakSedang)
            return "Menyerang";
        else if (skorSedikit && healthSedang && jarakDekat)
            return "Kabur";
        else if (skorSedikit && healthSedikit && jarakJauh)
            return "Menyerang";
        else if (skorSedikit && healthSedikit && jarakSedang)
            return "Menyerang";
        else if (skorSedikit && healthSedikit && jarakDekat)
            return "Menyerang";

        return "Menyerang";
    }
}
