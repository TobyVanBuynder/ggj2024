using UnityEngine;
using UnityEngine.InputSystem;

public class TicklingInput : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionReference keyActionA;
    public InputActionReference keyActionB;
    public InputActionReference keyActionX;
    public InputActionReference keyActionY;
    public InputActionReference keyActionLeft;
    public InputActionReference keyActionRight;
    public InputActionReference keyActionUp;
    public InputActionReference keyActionDown;
    
    private InputActionMap _ticklingActionMap;
    private TickleUI _tickleUI;

    void Awake()
    {
        _tickleUI = FindObjectOfType<TickleUI>();
        _ticklingActionMap = keyActionA.action.actionMap;
    }

    private void OnEnable()
    {
        keyActionA.action.started += OnKeyA;
        keyActionB.action.started += OnKeyB;
        keyActionX.action.started += OnKeyX;
        keyActionY.action.started += OnKeyY;
        keyActionLeft.action.started += OnKeyArrowLeft;
        keyActionRight.action.started += OnKeyArrowRight;
        keyActionUp.action.started += OnKeyArrowUp;
        keyActionDown.action.started += OnKeyArrowDown;
        _tickleUI.UIOpened += OnUIOpened;
        _tickleUI.UIClosed += OnUIClosed;
    }

    private void OnDisable()
    {
        _ticklingActionMap.Disable();
        
        keyActionA.action.started -= OnKeyA;
        keyActionB.action.started -= OnKeyB;
        keyActionX.action.started -= OnKeyX;
        keyActionY.action.started -= OnKeyY;
        keyActionLeft.action.started -= OnKeyArrowLeft;
        keyActionRight.action.started -= OnKeyArrowRight;
        keyActionUp.action.started -= OnKeyArrowUp;
        keyActionDown.action.started -= OnKeyArrowDown;
        _tickleUI.UIOpened -= OnUIOpened;
        _tickleUI.UIClosed -= OnUIClosed;
    }

    private void OnUIOpened()
    {
        _ticklingActionMap.Enable();
    }

    private void OnUIClosed()
    {
        _ticklingActionMap.Disable();
    }

    private void OnKeyA(InputAction.CallbackContext _)
    {
        _tickleUI.OnKeyPressed(TickleButtonType.A);
    }

    private void OnKeyB(InputAction.CallbackContext _)
    {
        _tickleUI.OnKeyPressed(TickleButtonType.B);
    }

    private void OnKeyX(InputAction.CallbackContext _)
    {
        _tickleUI.OnKeyPressed(TickleButtonType.X);
    }

    private void OnKeyY(InputAction.CallbackContext _)
    {
        _tickleUI.OnKeyPressed(TickleButtonType.Y);
    }

    private void OnKeyArrowLeft(InputAction.CallbackContext _)
    {
        _tickleUI.OnKeyPressed(TickleButtonType.LEFT);
    }

    private void OnKeyArrowRight(InputAction.CallbackContext _)
    {
        _tickleUI.OnKeyPressed(TickleButtonType.RIGHT);
    }

    private void OnKeyArrowUp(InputAction.CallbackContext _)
    {
        _tickleUI.OnKeyPressed(TickleButtonType.UP);
    }

    private void OnKeyArrowDown(InputAction.CallbackContext _)
    {
        _tickleUI.OnKeyPressed(TickleButtonType.DOWN);
    }
}
