using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [HideInInspector]
    public int currentHealth;

    public AudioSource playerHurtSound;
    public Slider playerHealthBar;
    public Button buttonUseHealth;
    public GameObject playerHurtEffect;
    public Text textHaveHealth;

    private GameManage gameManage;
    private NPCManage npcManage;

    private int startingHealth = 100;
    private int haveHealth;

    void Awake()
    {
        gameManage = GameObject.FindGameObjectWithTag("GameManage").GetComponent<GameManage>();
        npcManage = GameObject.FindGameObjectWithTag("NPCManage").GetComponent<NPCManage>();
        haveHealth = PlayerPrefs.GetInt("havehealth");
        if (haveHealth == 0)
        {
            textHaveHealth.color = Color.red;
            buttonUseHealth.interactable = false;
        }
        textHaveHealth.text = haveHealth.ToString();
        playerHealthBar = playerHealthBar.GetComponent<Slider>();
        playerHealthBar.enabled = false;
        currentHealth = startingHealth;
        SetPlayerHealth(startingHealth);
    }

    IEnumerator PlayerHurtEffect()
    {
        playerHurtEffect.SetActive(true);
        yield return new WaitForSeconds(.5f);
        playerHurtEffect.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        playerHurtSound.Play();
        StartCoroutine("PlayerHurtEffect");
        currentHealth -= damage;
        SetPlayerHealth(currentHealth);
        if (currentHealth <= 0) {
            npcManage.isOver = true;
            gameManage.Lose();
        }
    }

    public void SetPlayerHealth(int health)
    {
        playerHealthBar.value = health;
    }

    public void UseHealth()
    {
        int __hh;
        int _hh = PlayerPrefs.GetInt("havehealth");
        if(_hh != 0){
            PlayerPrefs.SetInt("havehealth", _hh-1);
            SetPlayerHealth(startingHealth);
            currentHealth = startingHealth;
            __hh = PlayerPrefs.GetInt("havehealth");
            textHaveHealth.text = __hh.ToString();
            if (__hh == 0)
            {
                textHaveHealth.color = Color.red;
                buttonUseHealth.interactable = false;
            }
        }
    }
}
