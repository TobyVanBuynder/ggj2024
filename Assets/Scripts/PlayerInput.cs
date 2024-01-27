using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference sprintAction;
    public InputActionReference tickleEngageAction;

    private bool _canMove = true;
    private Move _characterController;
    private TickleDetector _tickleDetector;
    
    private void Awake()
    {
        _characterController = GetComponent<Move>();
        _tickleDetector = GetComponentInChildren<TickleDetector>();
    }
    
    private void OnEnable()
    {
        moveAction.asset.Enable();
        jumpAction.asset.Enable();
        sprintAction.asset.Enable();
        tickleEngageAction.asset.Enable();
        
        jumpAction.action.started += JumpInputStarted;
        jumpAction.action.performed += JumpInputPerformed;
        jumpAction.action.canceled += JumpInputCanceled;
        tickleEngageAction.action.performed += EngageTickling;
        
        _tickleDetector.TicklingMinigameEnded += OnTicklingMinigameEnded;
    }

    private void OnTicklingMinigameEnded()
    {
        _canMove = true;
    }

    private void OnDisable()
    {
        jumpAction.action.started -= JumpInputStarted;
        jumpAction.action.performed -= JumpInputPerformed;
        jumpAction.action.canceled -= JumpInputCanceled;
        tickleEngageAction.action.canceled -= EngageTickling;
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
    
    private void EngageTickling(InputAction.CallbackContext _)
    {
        if(_tickleDetector.CanTickle())
        {
            _tickleDetector.EngageTickle();
            _canMove = false;
        }
    }
}  