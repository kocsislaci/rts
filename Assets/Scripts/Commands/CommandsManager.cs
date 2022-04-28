using System.Collections;
using System.Collections.Generic;
using RtsGameManager;
using UnityEngine;
using UnityEngine.InputSystem;


public class CommandsManager : MonoBehaviour
{
    private InputActions _inputActions;
    private InputAction _mousePosition;
    private InputAction _execute;

    private List<CommandExecuter> targetUnits = new List<CommandExecuter>();


    private void Awake()
    {
        _inputActions = new InputActions();
        _mousePosition = _inputActions.UnitActionMap.MousePosition;
        _execute = _inputActions.UnitActionMap.Execute;
    }

    private void OnEnable()
    {
        _execute.started += _ => GatherTargets();
        _execute.performed += _ => SelectAction();
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void GatherTargets()
    {
        if (RtsGameManager.GameManager.SELECTED_UNITS.Count != 0)
        {
            foreach (var selected in RtsGameManager.GameManager.SELECTED_UNITS)
            {
                targetUnits.Add(selected.gameObject.GetComponent<CommandExecuter>());
            }
        }
    }

    private void SelectAction()
    {
        SendMoveCommand();

        //todo: 
        //move
        //attack
        //gather
        //
        //reset and add
        //add and add like a queue
    }
    private void SendMoveCommand()
    {
        MoveCommand move = new MoveCommand(GetPosOnTerrain());
        foreach (var target in targetUnits)
        {
            target.AddCommand(move);
        }
    }


    private Vector3 GetPosOnTerrain()
    {
        Ray _ray = Camera.main.ScreenPointToRay(GetMousePosition());
        RaycastHit _raycastHit;
        if (Physics.Raycast(_ray, out _raycastHit, 1000f))
        {
            if (_raycastHit.transform.tag == "Terrain")
            {
                return _raycastHit.point;
            }
        }

        return new Vector3(0, 0, 0); //todo: try catch something
    }
    
    private Vector3 GetMousePosition()
    {
        return new Vector3(
            _mousePosition.ReadValue<Vector2>().x,
            _mousePosition.ReadValue<Vector2>().y,
            0.0f);
    }
}
