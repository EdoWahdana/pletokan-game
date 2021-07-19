using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private float hargaUpgradeWeapons = 100f;
    private float hargaHealth = 25f;
    public Text txthargaHealth;
    public Slider sliderDamage;
    public Slider sliderAmmoCapacity;
    private float damage;
    public Text textDamage;
    private float ammoCapacity;
    public Text textAmmoCapacity;
    private int haveHealth;
    public Text textHaveHealth;
    private float coin;
    public Text textCoin;
    public Text textWarning;
    public GameObject Warning;
    private float waitTimeWarning = 1f;
    private bool isWarning;

    // Start is called before the first frame update
    void Start()
    {
        damage = PlayerPrefs.GetFloat("damage");
        ammoCapacity = PlayerPrefs.GetFloat("ammocapacity");
        txthargaHealth.text = "$" + hargaHealth.ToString();
        sliderDamage = sliderDamage.GetComponent<Slider>();
        sliderAmmoCapacity = sliderAmmoCapacity.GetComponent<Slider>();

        sliderDamage.value = damage;
        textDamage.text = damage.ToString() + "%";
        sliderAmmoCapacity.value = ammoCapacity;
        textAmmoCapacity.text = ammoCapacity.ToString() + "%";
        haveHealth = PlayerPrefs.GetInt("havehealth");
        textHaveHealth.text = "You Have: " + haveHealth.ToString();
        coin = PlayerPrefs.GetFloat("coin");
        textCoin.text = "$" + coin.ToString();
    }

    public void BuyHealth()
    {
        if (coin >= hargaHealth){
            int _hh = PlayerPrefs.GetInt("havehealth");
            PlayerPrefs.SetInt("havehealth", _hh + 1);
            coin -= hargaHealth;
        } else{
            textWarning.text = "Coin Anda Tidak Cukup!";
            isWarning = true;
        }

        Start();
    }

    public void UpgradeWeapons()
    {
        if (coin >= hargaUpgradeWeapons)
        {
            float _d = PlayerPrefs.GetFloat("damage");
            float _ac = PlayerPrefs.GetFloat("ammocapacity");
            if(_d != 100f && _ac != 100f){
                PlayerPrefs.SetFloat("damage", _d + 10f);
                PlayerPrefs.SetFloat("ammocapacity", _ac + 10f);
                coin -= hargaUpgradeWeapons;
            } else{
                textWarning.text = "Level Pletokan Sudah Max!";
                isWarning = true;
            }
        } else{
            textWarning.text = "Coin Anda Tidak Cukup!";
            isWarning = true;
        }

        Start();
    }

    void Update()
    {
        if (isWarning){
            waitTimeWarning -= Time.deltaTime;
            Warning.SetActive(true);
            if(waitTimeWarning <= 0f){
                Warning.SetActive(false);
                isWarning = false;
                waitTimeWarning = 1f;
            }
        }
    }
}
