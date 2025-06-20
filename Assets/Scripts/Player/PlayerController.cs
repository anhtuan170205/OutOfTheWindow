using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private InputReader _input;

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
	private float _cinemachineTargetPitch;

	// player
	private float _speed;
	private float _rotationVelocity;
	private float _verticalVelocity;
	private float _terminalVelocity = 53.0f;

	// timeout deltatime
	private float _jumpTimeoutDelta;
	private float _fallTimeoutDelta;

	// player input
	private Vector2 _move;
	private Vector2 _look;
	private bool _jumpPressed;
	private bool _sprintPressed;


	private CharacterController _controller;
	private GameObject _mainCamera;

	private const float _threshold = 0.01f;

	private bool IsCurrentDeviceMouse => true;

	private void Awake()
	{
		// get a reference to our main camera
		if (_mainCamera == null)
		{
			_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
	}

    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}

	private void OnEnable()
	{
		_input.MoveEvent += OnMove;
		_input.LookEvent += OnLook;
		_input.JumpEvent += OnJump;
		_input.SprintEvent += OnSprint;
	}

	private void OnDisable()
	{
		_input.MoveEvent -= OnMove;
		_input.LookEvent -= OnLook;
		_input.JumpEvent -= OnJump;
		_input.SprintEvent -= OnSprint;
	}

	private void OnMove(Vector2 newMoveDirection)
	{
		_move = newMoveDirection;
	}
    private void OnLook(Vector2 newLookDirection)
    {
        _look = newLookDirection;
	}
	private void OnJump(bool newJumpState)
	{
		_jumpPressed = newJumpState;
	}
	private void OnSprint(bool newSprintState)
	{
		_sprintPressed = newSprintState;
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
        if (_look.sqrMagnitude >= _threshold)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            _cinemachineTargetPitch += _look.y * RotationSpeed * deltaTimeMultiplier;
            _rotationVelocity = _look.x * RotationSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Update Cinemachine camera target pitch
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

            // rotate the player left and right
            transform.Rotate(Vector3.up * _rotationVelocity);
        }
	}

	private void Move()
	{
		// set target speed based on move speed, sprint speed and if sprint is pressed
		float targetSpeed = _sprintPressed ? SprintSpeed : MoveSpeed;

		// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

		// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is no input, set the target speed to 0
		if (_move == Vector2.zero) targetSpeed = 0.0f;

		// a reference to the players current horizontal velocity
		float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

		float speedOffset = 0.1f;
		float inputMagnitude = _input.analogMovement ? _move.magnitude : 1f;

		// accelerate or decelerate to target speed
		if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
		{
			// creates curved result rather than a linear one giving a more organic speed change
			// note T in Lerp is clamped, so we don't need to clamp our speed
			_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

			// round speed to 3 decimal places
			_speed = Mathf.Round(_speed * 1000f) / 1000f;
		}
		else
		{
			_speed = targetSpeed;
		}

		// normalise input direction
		Vector3 inputDirection = new Vector3(_move.x, 0.0f, _move.y).normalized;

		// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is a move input rotate player when the player is moving
		if (_move != Vector2.zero)
		{
			// move
			inputDirection = transform.right * _move.x + transform.forward * _move.y;
		}

		// move the player
		_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
	}

	private void JumpAndGravity()
	{
		if (Grounded)
		{
			// reset the fall timeout timer
			_fallTimeoutDelta = FallTimeout;

			// stop our velocity dropping infinitely when grounded
			if (_verticalVelocity < 0.0f)
			{
				_verticalVelocity = -2f;
			}

			// Jump
			if (_jumpPressed && _jumpTimeoutDelta <= 0.0f)
			{
				// the square root of H * -2 * G = how much velocity needed to reach desired height
				_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
			}

			// jump timeout
			if (_jumpTimeoutDelta >= 0.0f)
			{
				_jumpTimeoutDelta -= Time.deltaTime;
			}
		}
		else
		{
			// reset the jump timeout timer
			_jumpTimeoutDelta = JumpTimeout;

			// fall timeout
			if (_fallTimeoutDelta >= 0.0f)
			{
				_fallTimeoutDelta -= Time.deltaTime;
			}

			// if we are not grounded, do not jump
			_jumpPressed = false;
		}

		// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
		if (_verticalVelocity < _terminalVelocity)
		{
			_verticalVelocity += Gravity * Time.deltaTime;
		}
	}

	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}

}