using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float turnSpeed = 10f;
    [SerializeField]
    private float forwardMoveSpeed = 2.5f;
    [SerializeField]
    private float backwardMoveSpeed = 2.0f;

    private CharacterController characterController;
    private Pletokan pletokan;
    private Vector2 runAxis;
    private Animator animator;
    private float speed = 1f;
    private bool isJump;

    public CameraController cameraController;
    public FixedJoystick joystick;
    public FixedTouchField touchField;
    public FixedButton shootButton;
    public FixedButton jumpButton;

    void Awake() 
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        pletokan = GetComponentInChildren<Pletokan>();
    }

    void Update() 
    {
        runAxis = joystick.Direction;

        isJump = jumpButton.Pressed;

        pletokan.isShoot = shootButton.Pressed;

        var horizontal = touchField.TouchDist.x;
        var vertical = touchField.TouchDist.y;
        var horizontalWalk = runAxis.x;
        var verticalWalk = runAxis.y;
        var movement = new Vector3(horizontal, 0, vertical);

        transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime);

        cameraController.vertical = vertical;

        animator.SetFloat("Horizontal", horizontalWalk);
        animator.SetFloat("Vertical", verticalWalk);

        if(verticalWalk != 0) {
            float moveSpeedToUse = verticalWalk > 0 ? forwardMoveSpeed : backwardMoveSpeed;
            characterController.SimpleMove(transform.forward * moveSpeedToUse * verticalWalk * speed);
            animator.SetFloat("Horizontal", horizontalWalk);
            animator.SetFloat("Vertical", verticalWalk);

        }

        if(horizontalWalk != 0) {
            characterController.SimpleMove(transform.right * forwardMoveSpeed * horizontalWalk * speed);
        }

        if(isJump) {
            StartCoroutine("Jump");
            StartCoroutine("Dejump");
            // Debug.Log("Setelah JUMP" + gameObject.transform.position);
            // Vector3 groundPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 30, gameObject.transform.position.z);
            // Debug.Log("Setelah GroundPos" + gameObject.transform.position);
            // gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, groundPos, Time.deltaTime);
            // Debug.Log("Setelah LERP" + gameObject.transform.position);
            isJump = false;
        }
    }

    IEnumerator Dejump()
    {
        Vector3 dejumpPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 30, gameObject.transform.position.z);
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, dejumpPos, Time.deltaTime);
        yield return new WaitForSeconds(2f);
    }

    IEnumerator Jump()
    {
        Vector3 jumpPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 30, gameObject.transform.position.z);
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, jumpPos, Time.deltaTime);
        yield return new WaitForSeconds(2f);
    }
    
}
