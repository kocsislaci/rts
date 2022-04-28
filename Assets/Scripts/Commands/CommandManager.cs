using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommandManager : MonoBehaviour
{
    private InputActions _inputActions;
    private InputAction _mousePosition;
    private InputAction _execute;
    
    private void Awake()
    {
        _inputActions = new InputActions();
        _mousePosition = _inputActions.UnitActionMap.MousePosition;
        _execute = _inputActions.UnitActionMap.Execute;
    }
    private void OnEnable()
    {
        _execute.started += _ => 
        _execute.performed += _ => 
        _execute.canceled += _ => 
        _mousePosition.performed += _ => 
        _inputActions.Enable();
    }
    private void OnDisable()
    {
        _inputActions.Disable();
    }
    
    
    
    
    
}
