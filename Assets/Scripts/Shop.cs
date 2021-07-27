using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private float priceUpgradeWeapons = 100f;
    private float priceHealth = 25f;
    public Text txtPriceHealth;
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

    public AudioSource buyHealthEffect;
    public AudioSource upWeaponsEffect;
    private AudioSource warning;

    // Start is called before the first frame update
    void Start()
    {
        damage = PlayerPrefs.GetFloat("damage");
        ammoCapacity = PlayerPrefs.GetFloat("ammocapacity");
        txtPriceHealth.text = "$" + priceHealth.ToString();
        sliderDamage = sliderDamage.GetComponent<Slider>();
        sliderAmmoCapacity = sliderAmmoCapacity.GetComponent<Slider>();

        sliderDamage.value = damage;
        textDamage.text = damage.ToString() + "%";
        sliderAmmoCapacity.value = ammoCapacity;
        textAmmoCapacity.text = ammoCapacity.ToString() + "%";
        haveHealth = PlayerPrefs.GetInt("havehealth");
        textHaveHealth.text = "You Have: " + haveHealth.ToString();
        coin = PlayerPrefs.GetFloat("coin");
        textCoin.text = "$" + ((int)coin).ToString();

        buyHealthEffect = buyHealthEffect.GetComponent<AudioSource>();
        upWeaponsEffect = upWeaponsEffect.GetComponent<AudioSource>();
        warning = GetComponent<AudioSource>();
    }

    public void BuyHealth()
    {
        if (coin >= priceHealth){
            buyHealthEffect.PlayOneShot(buyHealthEffect.clip);
            PlayerPrefs.SetFloat("coin", coin-priceHealth);
            float _c = PlayerPrefs.GetFloat("coin");
            textCoin.text = "$" + ((int)_c).ToString();
            int _hh = PlayerPrefs.GetInt("havehealth");
            PlayerPrefs.SetInt("havehealth", _hh + 1);
            int __hh = PlayerPrefs.GetInt("havehealth");
            textHaveHealth.text = "You Have: " + __hh.ToString();
        } else{
            warning.PlayOneShot(warning.clip);
            textWarning.text = "Coin Anda Tidak Cukup!";
            isWarning = true;
        }

        Start();
    }

    public void UpgradeWeapons()
    {
        if (coin >= priceUpgradeWeapons)
        {
            float _d = PlayerPrefs.GetFloat("damage");
            float _ac = PlayerPrefs.GetFloat("ammocapacity");
            if(_d != 100f && _ac != 100f){
                upWeaponsEffect.PlayOneShot(upWeaponsEffect.clip);
                PlayerPrefs.SetFloat("coin", coin - priceUpgradeWeapons);
                float _c = PlayerPrefs.GetFloat("coin");
                textCoin.text = "$" + ((int)_c).ToString();
                PlayerPrefs.SetFloat("damage", _d + 10f);
                PlayerPrefs.SetFloat("ammocapacity", _ac + 10f);
                float __d = PlayerPrefs.GetFloat("damage");
                float __ac = PlayerPrefs.GetFloat("ammocapacity");
                sliderDamage.value = __d;
                textDamage.text = __d.ToString() + "%";
                sliderAmmoCapacity.value = __ac;
                textAmmoCapacity.text = __ac.ToString() + "%";

            } else{
                warning.PlayOneShot(warning.clip);
                textWarning.text = "Level Pletokan Sudah Max!";
                isWarning = true;
            }
        } else{
            warning.PlayOneShot(warning.clip);
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
