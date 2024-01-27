using UnityEngine;
using UnityEngine.InputSystem;

public class TicklingInput : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionReference key1Action;
    public InputActionReference key2Action;
    
    private InputActionMap _ticklingActionMap;
    private TickleUI _tickleUI;

    void Awake()
    {
        _tickleUI = FindObjectOfType<TickleUI>();
        _ticklingActionMap = key1Action.action.actionMap;
    }

    private void OnEnable()
    {
        key1Action.action.started += OnKey1;
        key2Action.action.started += OnKey2;
        _tickleUI.UIOpened += OnUIOpened;
        _tickleUI.UIClosed += OnUIClosed;
    }

    private void OnDisable()
    {
        _ticklingActionMap.Disable();
        
        key1Action.action.started -= OnKey1;
        key2Action.action.started -= OnKey2;
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

    private void OnKey1(InputAction.CallbackContext _)
    {
        //Debug.Log("Key 1 pressed (A)");
        _tickleUI.OnKeyPressed(TickleButtonType.A);
    }

    private void OnKey2(InputAction.CallbackContext _)
    {
        //Debug.Log("Key 2 pressed (B)");
        _tickleUI.OnKeyPressed(TickleButtonType.B);
    }
}
