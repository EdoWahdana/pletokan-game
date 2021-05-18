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
    private Vector2 runAxis;
    private Animator animator;
    private float speed = 1f;

    public CameraController cameraController;
    public FixedJoystick joystick;
    public FixedTouchField touchField;

    void Awake() 
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update() 
    {
        runAxis = joystick.Direction;

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
    }
    // // Define Gameobject
    // public FixedJoystick moveJoystick;
    // public FixedTouchField touchField;

    // // Define player properties
    // public float moveSpeed = .2f;

    // void Update()
    // {
    //     UpdateMoveJoystick();
    // }

    // void UpdateMoveJoystick()
    // {
    //     float hoz = moveJoystick.Horizontal;
    //     float ver = moveJoystick.Vertical;

    //     Vector2 convertedXY = ConvertWithCamera(Camera.main.transform.position, hoz, ver);
    //     Vector3 direction = new Vector3(convertedXY.x, 0, convertedXY.y).normalized;
    //     transform.Translate(direction * moveSpeed, Space.World);
    // }

    // private Vector2 ConvertWithCamera(Vector3 cameraPos, float hor, float ver)
    // {
    //     Vector2 joyDirection = new Vector2(hor, ver).normalized;
    //     Vector2 camera2DPos = new Vector2(cameraPos.x, cameraPos.z);
    //     Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
    //     Vector2 cameraToPlayerDirection = (Vector2.zero - camera2DPos).normalized;
    //     float angle = Vector2.SignedAngle(cameraToPlayerDirection, new Vector2(0, 1));
    //     Vector2 finalDirection = RotateVector(joyDirection, -angle);
    //     return finalDirection;
    // }

    // public Vector2 RotateVector(Vector2 v, float angle)
    // {
    //     float radian = angle * Mathf.Deg2Rad;
    //     float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
    //     float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
    //     return new Vector2(_x, _y);
    // }
}
