using System;
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
    private InputAction _modifier;

    private List<CommandExecuter> targetUnits = new List<CommandExecuter>();


    private void Awake()
    {
        _inputActions = new InputActions();
        _mousePosition = _inputActions.UnitActionMap.MousePosition;
        _execute = _inputActions.UnitActionMap.Execute;
        _modifier = _inputActions.UnitActionMap.Modifier;
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

    private void SelectAction()
    {
        MoveRowStrategy rowStrategy = new MoveRowStrategy(); //TODO
        MoveBoxStrategy boxStrategy = new MoveBoxStrategy(); //TODO
        SendMoveCommand(boxStrategy);

        //todo: 
        //move to pos of other unit, 
        //move to closest part of a building
        //follow
        //attack
        //gather
        //build
        //
    }
    private void SendMoveCommand(IMoveStrategies strategy)
    {
        Vector3? targetPos = _GetPosOnTerrain();


        
            
        foreach (var target in targetUnits)
        {
            if (!_modifier.IsPressed())
            {
                target.ResetCommands();
            }
            MoveCommand move;
            if (targetPos == null)
            {
                move = new MoveCommand(target.transform.position);
            }
            else
            {
                Vector3 calculatedTargetPos = strategy.CalculateMovementStrategy(targetPos!, targetUnits.IndexOf(target));
                move = new MoveCommand(calculatedTargetPos);
            }
            target.AddCommand(move);
        }
        
    }
    private void GatherTargets()
    {
        targetUnits.Clear();
        if (RtsGameManager.GameManager.SELECTED_UNITS.Count != 0)
        {
            foreach (var selected in RtsGameManager.GameManager.SELECTED_UNITS)
            {
                targetUnits.Add(selected.gameObject.GetComponent<CommandExecuter>());
            }
        }
    }
    private Vector3? _GetPosOnTerrain()
    {
        Ray _ray = Camera.main.ScreenPointToRay(_GetMousePosition());
        RaycastHit _raycastHit;
        int terrainLayerMask = LayerMask.GetMask("Terrain");
        if (Physics.Raycast(_ray, out _raycastHit, 1000f, terrainLayerMask))
        {
            return _raycastHit.point;
        }
        return null;
    }
    private Vector3 _GetPosOnFollow()
    {
        return Vector3.zero; //TODO
    }
    private Vector3 _GetMousePosition()
    {
        return new Vector3(
            _mousePosition.ReadValue<Vector2>().x,
            _mousePosition.ReadValue<Vector2>().y,
            0.0f);
    }
}

