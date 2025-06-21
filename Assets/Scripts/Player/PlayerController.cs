using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader input;

    [Header("Player")]
    public float MoveSpeed = 4.0f;
    public float SprintSpeed = 6.0f;
    public float RotationSpeed = 1.0f;
    public float SpeedChangeRate = 10.0f;

    [Space(10)]
    public float JumpHeight = 1.2f;
    public float Gravity = -15.0f;

    [Space(10)]
    public float JumpTimeout = 0.1f;
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.5f;
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    public GameObject CinemachineCameraTarget;
    public float TopClamp = 90.0f;
    public float BottomClamp = -90.0f;

    // cinemachine
    private float cinemachineTargetPitch;

    // player
    private float speed;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    // player input
    private Vector2 move;
    private Vector2 look;
    private bool jumpPressed;
    private bool sprintPressed;


    private CharacterController controller;
    private GameObject mainCamera;

    private const float threshold = 0.01f;

    private bool IsCurrentDeviceMouse => true;

    private void Awake()
    {
        // get a reference to our main camera
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        // reset our timeouts on start
        jumpTimeoutDelta = JumpTimeout;
        fallTimeoutDelta = FallTimeout;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        input.MoveEvent += OnMove;
        input.LookEvent += OnLook;
        input.JumpEvent += OnJump;
        input.SprintEvent += OnSprint;
    }

    private void OnDisable()
    {
        input.MoveEvent -= OnMove;
        input.LookEvent -= OnLook;
        input.JumpEvent -= OnJump;
        input.SprintEvent -= OnSprint;
    }

    private void OnMove(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }
    private void OnLook(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }
    private void OnJump(bool newJumpState)
    {
        jumpPressed = newJumpState;
    }
    private void OnSprint(bool newSprintState)
    {
        sprintPressed = newSprintState;
    }

    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }

    private void CameraRotation()
    {
        // if there is an input
        if (look.sqrMagnitude >= threshold)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            cinemachineTargetPitch += look.y * RotationSpeed * deltaTimeMultiplier;
            rotationVelocity = look.x * RotationSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

            // Update Cinemachine camera target pitch
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0.0f, 0.0f);

            // rotate the player left and right
            transform.Rotate(Vector3.up * rotationVelocity);
        }
    }

    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = sprintPressed ? SprintSpeed : MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = input.analogMovement ? move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }

        // normalise input direction
        Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (move != Vector2.zero)
        {
            // move
            inputDirection = transform.right * move.x + transform.forward * move.y;
        }

        // move the player
        controller.Move(inputDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            fallTimeoutDelta = FallTimeout;

            // stop our velocity dropping infinitely when grounded
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            // Jump
            if (jumpPressed && jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }

            // jump timeout
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }

            // if we are not grounded, do not jump
            jumpPressed = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

}