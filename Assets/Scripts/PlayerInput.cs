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
    private TickleSpotDetector _tickleSpotSpotDetector;
    private InputActionMap _platformingActionMap;
    
    private void Awake()
    {
        _characterController = GetComponent<Move>();
        _tickleSpotSpotDetector = GetComponentInChildren<TickleSpotDetector>();
    }
    
    private void OnEnable()
    {
        _platformingActionMap = moveAction.action.actionMap;
        _platformingActionMap.Enable();
        
        jumpAction.action.started += JumpInputStarted;
        jumpAction.action.performed += JumpInputPerformed;
        jumpAction.action.canceled += JumpInputCanceled;
        tickleEngageAction.action.performed += EngageTickling;
        
        _tickleSpotSpotDetector.TicklingMinigameEnded += OnTicklingMinigameEnded;
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
        if(_tickleSpotSpotDetector.CanTickle())
        {
            _tickleSpotSpotDetector.EngageTickle();
            _platformingActionMap.Disable();
        }
    }
}  