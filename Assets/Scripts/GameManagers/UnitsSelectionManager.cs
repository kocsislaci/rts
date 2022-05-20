using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class UnitsSelectionManager : MonoBehaviour
{
    private bool _isDraggingMouseBox = false;
    private Vector3 _dragStartPosition;

    public UnityEvent selectionEvent;
    
    Ray _ray;
    RaycastHit _raycastHit;
    
    private InputActions _inputActions;
    private InputAction _mousePosition;
    private InputAction _select;
    
    private void Awake()
    {
        selectionEvent = new UnityEvent();
        
        _inputActions = new InputActions();
        _mousePosition = _inputActions.UnitActionMap.MousePosition;
        _select = _inputActions.UnitActionMap.Select;
    }
    private void OnEnable()
    {
        _select.started += _ => DraggingStarts();
        _select.performed += _ => SelectSingleUnit();
        _select.canceled += _ => DraggingStops();
        _mousePosition.performed += _ => Dragging();
        _inputActions.Enable();
    }
    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void SelectSingleUnit()
    {
        _DeselectAllUnits();
        _ray = Camera.main.ScreenPointToRay(GetMousePosition());
        if (Physics.Raycast(_ray, out _raycastHit, 1000f))
        {
            if (_raycastHit.transform.tag == "Unit")
            {
                _raycastHit.transform.gameObject.GetComponent<UnitSelectionController>().Select();
            }
        }
        selectionEvent.Invoke();
    }
    private void DraggingStarts()
    {
        _isDraggingMouseBox = true;
        _dragStartPosition = GetMousePosition();
    }
    private void Dragging()
    {
        if (_isDraggingMouseBox && _dragStartPosition != GetMousePosition())
        {
            _SelectUnitsInDraggingBox();
        }
    }
    private void DraggingStops()
    {
        _isDraggingMouseBox = false;
    }
    
    
    private void _SelectUnitsInDraggingBox()
    {
        Bounds selectionBounds = RtsUtils.SelectionUtils.GetViewportBounds(Camera.main, _dragStartPosition, GetMousePosition());
        GameObject[] selectableUnits = GameObject.FindGameObjectsWithTag("Unit");
        bool inBounds;
        foreach (GameObject unit in selectableUnits)
        {
            inBounds = selectionBounds.Contains(Camera.main.WorldToViewportPoint(unit.transform.position));
            if (inBounds)
            {
                unit.GetComponent<UnitSelectionController>().Select();
            }
            else
                unit.GetComponent<UnitSelectionController>().Deselect();
            selectionEvent.Invoke();
        }
    }
    private void _DeselectAllUnits()
    {
        List<UnitSelectionController> selectedUnits = new List<UnitSelectionController>(RtsGameManager.GameManager.SELECTED_UNITS);
        foreach (UnitSelectionController um in selectedUnits)
            um.Deselect();
    }
    void OnGUI()
    {
        if (_isDraggingMouseBox)
        {
            // Create a rect from both mouse positions
            var rect = RtsUtils.SelectionUtils.GetScreenRect(_dragStartPosition, GetMousePosition());
            RtsUtils.SelectionUtils.DrawScreenRect(rect, new Color(0.5f, 1f, 0.4f, 0.2f));
            RtsUtils.SelectionUtils.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
        }
    }
    private Vector3 GetMousePosition()
    {
        return new Vector3(
            _mousePosition.ReadValue<Vector2>().x,
            _mousePosition.ReadValue<Vector2>().y,
            0.0f);
    }
}