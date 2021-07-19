using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [Range(0.5f, 1.5f)]
    private float fireRate = 1f;

    [SerializeField]
    private AudioSource shootAudio;

    private float timer;
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

    private int targetCount = 1;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        shootAudio = shootAudio.GetComponent<AudioSource>();
    }

    void Update()
    {
        analog = joystick.Direction;

        isShoot = shootButton.Pressed;

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


        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            if (isShoot)
            {
                animator.SetBool("isShoot", true);
                timer = 0f;
                Fire();
            }
        }

        if(isTarget == true){
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

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * .5f);
        RaycastHit target;
        if (Physics.Raycast(ray, out target, 100))
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
        // shootBurst.Play();
        shootAudio.PlayOneShot(shootAudio.clip);

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * .5f);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100))
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
}
