using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float turnSpeed = 5f;
    [SerializeField]
    private float forwardMoveSpeed = 2.5f;
    [SerializeField]
    private float backwardMoveSpeed = 2.0f;

    private CharacterController characterController;
    private Pletokan pletokan;
    private Animator animator;
    private Vector2 analog;
    private float speed = 1f;

    public CameraController cameraController;
    public FloatingJoystick joystick;
    public FixedTouchField touchField;
    public FixedButton shootButton;

    public GameObject[] attackArea;


    public float jumpPower = 5f;

    private float moveSpeedToUse;

    private bool isReload;
    private float waitReload = 1f;

    public bool isOver = false;
    private static float speedAttack = 1.5f;
    private float m_hitDelay = speedAttack;
    private PlayerHealth playerHealth;
    private int level;
    public GameObject warning;

    void Awake() 
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        level = PlayerPrefs.GetInt("checklevel");
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        pletokan = GetComponentInChildren<Pletokan>();
    }

    void Update() 
    {
        analog = joystick.Direction;

        //Jika zoom kurangi sensitivitas
        if (cameraController.isZoom)
        {
            turnSpeed = 1f;
        }
        else {
            turnSpeed = 5f;
        }

        //Blok fungsi untuk shoot
        pletokan.isShoot = shootButton.Pressed;
        if (pletokan.isShoot){
            animator.SetBool("isShoot", true);
        }else{
            animator.SetBool("isShoot", false);
        }

        //Blok fungsi untuk reload ammo
        if (isReload)
        {
            animator.SetBool("isReload", true);
            pletokan.fireRate += waitReload;
            waitReload -= Time.deltaTime;
            if (waitReload <= 0) { 
                isReload = false;
                pletokan.fireRate = .5f;
            }
        }
        else {
            animator.SetBool("isReload", false);
            waitReload = 1f;
        }

        var horizontal = touchField.TouchDist.x;
        var vertical = touchField.TouchDist.y;
        var movement = new Vector3(horizontal, 0, vertical);

        transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime);

        cameraController.vertical = vertical;

        animator.SetFloat("Horizontal", analog.x);
        animator.SetFloat("Vertical", analog.y);

        if(analog.y != 0) {
            VerticalWalk();
        }

        if(analog.x != 0) {
            HorizontalWalk();
        }

        if(level < 6) {
            if ((Vector3.Distance(attackArea[0].transform.position, gameObject.transform.position)) <= 25f
                            || (Vector3.Distance(attackArea[1].transform.position, gameObject.transform.position)) <= 25f
                            || (Vector3.Distance(attackArea[2].transform.position, gameObject.transform.position)) <= 20f) {
                if (!isOver) {
                    AttackPlayer();
                    warning.SetActive(true);
                }
            } else {
                warning.SetActive(false);
            }
        }

    }

    private void AttackPlayer()
    {
        m_hitDelay -= Time.deltaTime;
        if (m_hitDelay <= 0f)
        {
            playerHealth.TakeDamage(10);
            m_hitDelay = speedAttack;
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
     
        if(!isRun){
            speed = 1f;
            animator.speed = 1f;
        }
    }

    public void Reload()
    {
        pletokan.isReload = true;
        isReload = true;
    }
}
