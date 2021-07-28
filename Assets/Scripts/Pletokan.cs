using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pletokan : MonoBehaviour
{

    public ParticleSystem shootBurst;
    public GameObject targetPointInactive;
    public GameObject targetPointActive;
    public AudioSource shootAudio;
    public Text ammoText;
    public float fireRate = .5f;
    public float ammo;
    public bool isShoot, isReload;

    private float timer, maxAmmo, damage;

    void Awake()
    {
        maxAmmo = PlayerPrefs.GetFloat("ammocapacity") / 2;
        damage = PlayerPrefs.GetFloat("damage");
        ammoText.text = maxAmmo.ToString();
        ammo = maxAmmo;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= fireRate) {
            if (isShoot && ammo > 0)
            {
                timer = 0f;
                Fire();
            }
        }

        if (isReload){
            ammo = maxAmmo;
            ammoText.text = ammo.ToString();
            isReload = false;
        }

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * .5f);
        RaycastHit target;
        if (Physics.Raycast(ray, out target, 100))
        {
            if (target.transform.parent.tag == "NPC")
            {
                targetPointInactive.SetActive(false);
                targetPointActive.SetActive(true);
            }
            else
            {
                targetPointInactive.SetActive(true);
                targetPointActive.SetActive(false);
            }
        }
        else
        {
            targetPointInactive.SetActive(true);
            targetPointActive.SetActive(false);
        }
    }    

    private void Fire()
    {
        ammo--;
        shootBurst.Play();
        shootAudio.Play();
        ammoText.text = ammo.ToString();

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * .5f);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, 100)) {
            var enemyHealth = hitInfo.collider.GetComponent<EnemyHealth>();
            if(enemyHealth && enemyHealth.currentHealth != 0) {
                enemyHealth.TakeDamage((int) damage);
            }
        }
    }

}
