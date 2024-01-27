using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference sprintAction;
    public InputActionReference tickleEngageAction;

    private Move _characterController;
    private TickleDetector _tickleDetector;
    private InputActionMap _platformingActionMap;
    
    private void Awake()
    {
        _characterController = GetComponent<Move>();
        _tickleDetector = GetComponentInChildren<TickleDetector>();
    }
    
    private void OnEnable()
    {
        _platformingActionMap = moveAction.action.actionMap;
        _platformingActionMap.Enable();
        
        jumpAction.action.started += JumpInputStarted;
        jumpAction.action.performed += JumpInputPerformed;
        jumpAction.action.canceled += JumpInputCanceled;
        tickleEngageAction.action.performed += EngageTickling;
        
        _tickleDetector.TicklingMinigameEnded += OnTicklingMinigameEnded;
    }

    private void OnDisable()
    {
        _platformingActionMap.Disable();
        
        jumpAction.action.started -= JumpInputStarted;
        jumpAction.action.performed -= JumpInputPerformed;
        jumpAction.action.canceled -= JumpInputCanceled;
        tickleEngageAction.action.canceled -= EngageTickling;
    }

    private void OnTicklingMinigameEnded()
    {
        moveAction.action.actionMap.Enable();
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
            _platformingActionMap.Disable();
        }
    }
}  