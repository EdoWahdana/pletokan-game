using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{

    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetPlayerMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetPlayerHealth(int health)
    {
        slider.value = health;
    }
}