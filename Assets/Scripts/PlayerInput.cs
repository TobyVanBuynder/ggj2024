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
        
        jumpAction.action.performed += JumpInputPerformed;
        jumpAction.action.canceled += JumpInputCanceled;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= JumpInputPerformed;
        jumpAction.action.canceled -= JumpInputCanceled;
    }

    private void Update()
    {
        _characterController.MoveInput = moveAction.action.ReadValue<float>();
        _characterController.IsSprinting = sprintAction.action.IsPressed();
    }

    private void JumpInputPerformed(InputAction.CallbackContext _)
    {
        _characterController.TryJump();
    }

    private void JumpInputCanceled(InputAction.CallbackContext _)
    {
        _characterController.InterruptJump();
    }

}  