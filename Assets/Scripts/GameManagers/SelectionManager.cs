using System.Collections.Generic;
using GameManagers;
using Unit;
using Unit.Building;
using Unit.Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using CharacterController = Unit.Character.CharacterController;


public class SelectionManager : MonoBehaviour
{
    public static bool IsMouseOverUi
    {
        get
        {
            // [Only works well while there is not PhysicsRaycaster on the Camera)
            //EventSystem eventSystem = EventSystem.current;
            //return (eventSystem != null && eventSystem.IsPointerOverGameObject());
 
            // [Works with PhysicsRaycaster on the Camera. Requires New Input System. Assumes mouse.)
            if (EventSystem.current == null)
            {
                return false;
            }
            RaycastResult lastRaycastResult = ((InputSystemUIInputModule)EventSystem.current.currentInputModule).GetLastRaycastResult(Mouse.current.deviceId);
            const int uiLayer = 5;
            return lastRaycastResult.gameObject != null && lastRaycastResult.gameObject.layer == uiLayer;
        }
    }
    
    private bool _isDraggingMouseBox = false;
    private Vector3 _dragStartPosition;
    
    public UnityEvent<Unit.Unit> selectionEvent;
    public UnityEvent deselectionEvent;

    private Ray _ray;
    private RaycastHit _raycastHit;
    
    private InputActions _inputActions;
    private InputAction _mousePosition;
    private InputAction _select;
    
    private void Awake()
    {
        selectionEvent = new UnityEvent<Unit.Unit>();
        deselectionEvent = new UnityEvent();

        _inputActions = new InputActions();
        _mousePosition = _inputActions.UnitActionMap.MousePosition;
        _select = _inputActions.UnitActionMap.Select;
    }
    private void OnEnable()
    {
        _select.started += _ => DraggingStarts();
        _select.performed += _ => SelectSingle();
        _select.canceled += _ => DraggingStops();
        _mousePosition.performed += _ => Dragging();
        _inputActions.Enable();
    }
    private void OnDisable()
    {
        _inputActions.Disable();
    }
    
    private void SelectSingle()
    {
        if (IsMouseOverUi) return;
        
        _ray = Camera.main.ScreenPointToRay(GetMousePosition());
        _DeselectAllUnits();
        _DeselectAllBuilding();
        deselectionEvent.Invoke();
        if (Physics.Raycast(_ray, out _raycastHit, 1000f))
        {
            if (_raycastHit.transform.CompareTag("Building"))
            {
                _raycastHit.transform.gameObject.GetComponent<BuildingSelectionController>().Select();
                selectionEvent.Invoke(_raycastHit.transform.gameObject.GetComponentInChildren<BuildingController>().selfClass);
            }
            else if (_raycastHit.transform.CompareTag("Unit"))
            {
                _raycastHit.transform.gameObject.GetComponent<CharacterSelectionController>().Select();
                selectionEvent.Invoke(_raycastHit.transform.gameObject.GetComponent<CharacterController>().selfClass);
            }
        }
    }
    private void DraggingStarts()
    {
        if (IsMouseOverUi) return;

        _isDraggingMouseBox = true;
        _dragStartPosition = GetMousePosition();
        // _DeselectAllBuilding(); // TODO: Is it necessary?
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
        Bounds selectionBounds = SelectionBoundingBoxDrawer.GetViewportBounds(Camera.main, _dragStartPosition, GetMousePosition());
        GameObject[] selectableUnits = GameObject.FindGameObjectsWithTag("Unit");
        bool inBounds;
        foreach (GameObject unit in selectableUnits)
        {
            inBounds = selectionBounds.Contains(Camera.main.WorldToViewportPoint(unit.transform.position));
            if (inBounds)
            {
                unit.GetComponent<CharacterSelectionController>().Select();
                if (GameManager.SELECTED_CHARACTERS.Contains( (CharacterSelectionController)_raycastHit.transform.gameObject.GetComponent<UnitSelectionController>()))
                {
                    selectionEvent.Invoke(_raycastHit.transform.gameObject.GetComponent<CharacterController>().selfClass);
                }
            }
            else
            {
                unit.GetComponent<CharacterSelectionController>().Deselect();

            }
        }
    }
    private void _DeselectAllUnits()
    {
        List<CharacterSelectionController> selectedUnits = new List<CharacterSelectionController>(GameManagers.GameManager.SELECTED_CHARACTERS);
        foreach (CharacterSelectionController usc in selectedUnits)
            usc.Deselect();
    }
    private void _DeselectAllBuilding()
    {
        List<BuildingSelectionController> selectedBuildings = new List<BuildingSelectionController>(GameManagers.GameManager.SELECTED_BUILDINGS);
        foreach (BuildingSelectionController bsc in selectedBuildings)
            bsc.Deselect();
    }

    /*
     * 
     */
    void OnGUI()
    {
        if (_isDraggingMouseBox)
        {
            var rect = SelectionBoundingBoxDrawer.GetScreenRect(_dragStartPosition, GetMousePosition());
            SelectionBoundingBoxDrawer.DrawScreenRect(rect, new Color(0.5f, 1f, 0.4f, 0.2f));
            SelectionBoundingBoxDrawer.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
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
