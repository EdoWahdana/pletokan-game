using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private float turnSpeed = 5f;
    [SerializeField]
    private float forwardMoveSpeed = 2.5f;
    [SerializeField]
    private float backwardMoveSpeed = 2.0f;

    private CharacterController characterController;
    private Animator animator;
    private Vector2 analog;
    private float speed = 1f;

    public CameraController cameraController;
    public FloatingJoystick joystick;
    public FixedTouchField touchField;
    public FixedButton shootButton;

    public float jumpPower = 5f;

    private float moveSpeedToUse;

    [SerializeField]
    private AudioSource shootAudio;

    public bool isShoot;

    public GameObject targetPointInactive;
    public GameObject targetPointActive;

    public GameObject popUpHebat;
    public GameObject popUpCobaLagi;
    private float popUpTime = 1f;
    private bool isTarget;
    private bool isNotTarget;

    public GameObject popUpEndGame;
    private float popUpTimeEndGame = 6f;

    private int targetCount = 2;

    private int startAmmo = 1;
    private int ammo;
    public Text textAmmo;
    public GameObject popUpReloadWarning;
    private bool isReloadWarning;

    private bool isReload;
    private float waitReload = 1f;

    void Awake()
    {
        ammo = startAmmo;
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        shootAudio = shootAudio.GetComponent<AudioSource>();
    }

    void Start()
    {
        textAmmo.text = ammo.ToString();
    }

    void Update()
    {
        analog = joystick.Direction;

        //isShoot = shootButton.Pressed;

        if(!isShoot)
        {
            animator.SetBool("isShoot", false);
        }

        var horizontal = touchField.TouchDist.x;
        var vertical = touchField.TouchDist.y;
        var movement = new Vector3(horizontal, 0, vertical);

        transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime);

        cameraController.vertical = vertical;

        animator.SetFloat("Horizontal", analog.x);
        animator.SetFloat("Vertical", analog.y);

        if (analog.y != 0)
        {
            VerticalWalk();
        }

        if (analog.x != 0)
        {
            HorizontalWalk();
        }

        if (isShoot)
        {
            if(ammo >= 1)
            {
                animator.SetBool("isShoot", true);
                Fire();
                ammo -= 1;
                Start();
            }
            else
            {
                isReloadWarning = true;
            }

            isShoot = false;
        }

        if (isTarget == true){
            popUpHebat.SetActive(true);
            popUpTime -= Time.deltaTime;
            if (popUpTime <= 0)
            {
                popUpHebat.SetActive(false);
                popUpTime = 1f;
                isTarget = false;
            }
        }

        if(isNotTarget == true){
            popUpCobaLagi.SetActive(true);
            popUpTime -= Time.deltaTime;
            if (popUpTime <= 0)
            {
                popUpCobaLagi.SetActive(false);
                popUpTime = 1f;
                isNotTarget = false;
            }
        }

        if (isReloadWarning == true)
        {
            popUpReloadWarning.SetActive(true);
            popUpTime -= Time.deltaTime;
            if (popUpTime <= 0)
            {
                popUpReloadWarning.SetActive(false);
                popUpTime = 1f;
                isReloadWarning = false;
            }
        }

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * .5f);
        RaycastHit target;
        if (Physics.Raycast(ray, out target, 50))
        {
            if (target.transform.tag == "NPC")
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

        if(targetCount == 0)
        {
            popUpEndGame.SetActive(true);
            popUpTimeEndGame -= Time.deltaTime;
            if(popUpTimeEndGame <= 0){
                popUpEndGame.SetActive(false);
                PlayerPrefs.SetInt("indextimeline", 1);
                SceneManager.LoadScene("CutScene");
            }
        }

        //Blok fungsi untuk reload ammo
        if (isReload)
        {
            animator.SetBool("isReload", true);
            waitReload -= Time.deltaTime;
            if (waitReload <= 0)
            {
                isReload = false;
            }
        }
        else
        {
            animator.SetBool("isReload", false);
            waitReload = 1f;
        }
    }

    private void HorizontalWalk()
    {
        characterController.SimpleMove(transform.right * forwardMoveSpeed * analog.x * speed);
    }
    private void VerticalWalk()
    {
        moveSpeedToUse = analog.y > 0 ? forwardMoveSpeed : backwardMoveSpeed;
        characterController.SimpleMove(transform.forward * moveSpeedToUse * analog.y * speed);
        animator.SetFloat("Horizontal", analog.x);
        animator.SetFloat("Vertical", analog.y);
    }

    public void Run(bool isRun)
    {
        VerticalWalk();
        speed = 2f;
        animator.speed = 1.5f;

        if (!isRun)
        {
            speed = 1f;
            animator.speed = 1f;
        }
    }

    private void Fire()
    {
        //shootBurst.Play();
        shootAudio.PlayOneShot(shootAudio.clip);

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * .5f);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 50))
        {
            if(hitInfo.collider.tag == "NPC")
            {
                Destroy(hitInfo.collider.transform.parent.gameObject);
                targetCount--;
                isTarget = true;
            }
            else
            {
                isNotTarget = true;
            }
        }
        else
        {
            isNotTarget = true;
        }
    }

  public void Skip()
  {
    PlayerPrefs.SetInt("indextimeline", 1);
    SceneManager.LoadScene("CutScene");
  }

    public void Shoot()
    {
        isShoot = true;
    }

    public void Reload()
    {
        ammo = startAmmo;
        Start();
        isReload = true;
    }

}
