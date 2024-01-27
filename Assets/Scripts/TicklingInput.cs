using UnityEngine;
using UnityEngine.InputSystem;

public class TicklingInput : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionReference key1Action;
    public InputActionReference key2Action;
    
    private InputActionMap _ticklingActionMap;
    private TickleUI _tickleUI;

    void Start()
    {
        _tickleUI = FindObjectOfType<TickleUI>();
        key1Action.action.started += OnKey1;
        key2Action.action.started += OnKey2;
        _ticklingActionMap = key1Action.action.actionMap;
    }

    private void OnEnable()
    {
        //_ticklingActionMap.Enable();
    }

    private void OnDisable()
    {
        _ticklingActionMap.Disable();
    }

    private void OnKey1(InputAction.CallbackContext _)
    {
        Debug.Log("Key 1 pressed (A)");
        _tickleUI.OnKeyPressed(TickleButtonType.A);
    }

    private void OnKey2(InputAction.CallbackContext _)
    {
        Debug.Log("Key 2 pressed (B)");
        _tickleUI.OnKeyPressed(TickleButtonType.B);
    }
}
