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
        
        jumpAction.action.performed += JumpInputPerformed;
        tickleEngageAction.action.performed += EngageTickling;
        
        _tickleSpotSpotDetector.TicklingMinigameEnded += OnTicklingMinigameEnded;
    }

    private void OnDisable()
    {
        _platformingActionMap.Disable();
        
        jumpAction.action.performed -= JumpInputPerformed;
        tickleEngageAction.action.performed -= EngageTickling;
    }

    private void Update()
    {
        if (!_platformingActionMap.enabled) return;
        
        _characterController.MoveInput = moveAction.action.ReadValue<Vector2>();
        _characterController.IsSprinting = sprintAction.action.IsPressed();
    }

    private void JumpInputPerformed(InputAction.CallbackContext _)
    {
        _characterController.PrepareJump();
    }
    
    private void EngageTickling(InputAction.CallbackContext _)
    {
        if(_tickleSpotSpotDetector.CanTickle())
        {
            _tickleSpotSpotDetector.EngageTickle();
            _platformingActionMap.Disable();
        }
    }

    private void OnTicklingMinigameEnded()
    {
        _platformingActionMap.Enable();
    }
}  