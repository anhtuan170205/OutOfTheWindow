using UnityEngine;
using UnityEngine.InputSystem;
using System;
using static PlayerControls;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;
    public event Action<bool> JumpEvent;
    public event Action<bool> SprintEvent;
    public event Action<bool> ShootEvent;
    public event Action<bool> ReloadEvent;
    public event Action<bool> SwapEvent;
	public bool analogMovement;
    public bool cursorLocked = true;
    public bool cursorInputForLook = true; 

    private PlayerControls playerControls;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Player.SetCallbacks(this);
        }
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            JumpEvent?.Invoke(false);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SprintEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            SprintEvent?.Invoke(false);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShootEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            ShootEvent?.Invoke(false);
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ReloadEvent?.Invoke(true);
        }
    }

    public void OnSwap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwapEvent?.Invoke(true);
        }
    }

    private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
