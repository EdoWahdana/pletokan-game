using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pletokan : MonoBehaviour
{

    [SerializeField]
    [Range(0.5f, 1.5f)]
    private float fireRate = 1f;

    [SerializeField]
    [Range(1, 100)]
    private float damage = 1;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private ParticleSystem shootBurst;

    [SerializeField]
    private AudioSource shootAudio;

    private float timer;
    
    public bool isShoot; 

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= fireRate) {
            if(isShoot) {
                timer = 0f; 
                Fire();
            }
        }
    }    

    private void Fire()
    {
        // shootBurst.Play();
        // shootAudio.Play();

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * .5f);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, 100)) {
            Debug.Log("Hit : " + hitInfo.collider.name);
            // var enemyHealth = hitInfo.collider.GetComponent<EnemyHealth>();

            // if(enemyHealth.currentHealth != 0) {
            //     enemyHealth.TakeDamage(damage);
            // }
        }
    }

}
