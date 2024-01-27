using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TicklingInput : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionReference key1Action;
    public InputActionReference key2Action;
    
    private InputActionMap _ticklingActionMap;

    private void OnEnable()
    {
        _ticklingActionMap = key1Action.action.actionMap;
        _ticklingActionMap.Enable();

        key1Action.action.started += OnKey1;
        key2Action.action.started += OnKey2;
    }

    private void OnDisable()
    {
        _ticklingActionMap.Disable();
    }

    private void OnKey1(InputAction.CallbackContext _)
    {
        Debug.Log("Key 1 pressed");
    }

    private void OnKey2(InputAction.CallbackContext _)
    {
        Debug.Log("Key 2 pressed");
    }
}
