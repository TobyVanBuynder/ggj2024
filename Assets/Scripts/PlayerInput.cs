using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference sprintAction;

    private Move _characterController;
    
    private void Awake()
    {
        _characterController = GetComponent<Move>();
    }
    
    private void OnEnable()
    {
        moveAction.asset.Enable();
        jumpAction.asset.Enable();
        sprintAction.asset.Enable();
        
        jumpAction.action.started += JumpInputStarted;
        jumpAction.action.performed += JumpInputPerformed;
        jumpAction.action.canceled += JumpInputCanceled;
    }

    private void OnDisable()
    {
        jumpAction.action.started -= JumpInputStarted;
        jumpAction.action.performed -= JumpInputPerformed;
        jumpAction.action.canceled -= JumpInputCanceled;
    }

    private void Update()
    {
        _characterController.MoveInput = moveAction.action.ReadValue<Vector2>();
        _characterController.IsSprinting = sprintAction.action.IsPressed();
    }

    private void JumpInputStarted(InputAction.CallbackContext _)
    {
        _characterController.AnticipateJump();
    }

    private void JumpInputPerformed(InputAction.CallbackContext _)
    {
        _characterController.ReadyToJump();
    }

    private void JumpInputCanceled(InputAction.CallbackContext _)
    {
        _characterController.InterruptJump();
    }

}  