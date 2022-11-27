using System.Collections.Generic;
using GameManagers.SelectionManager;
using Unit;
using Unit.Building;
using Unit.ResourceObject;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using CharacterController = Unit.Character.CharacterController;

namespace GameManagers.UserInputManager
{
    public class UserInputManager : MonoBehaviour
    {
        /*
         * Command related variables
         */
        private Vector3 lastClickPosition;
        private UnitController lastTarget;
        private ResourceController lastResourceTarget;
        
        /*
         * Selection related variables
         */
        private bool _isDraggingMouseBox = false;
        private Vector3 _dragStartPosition;
    
        /*
         * Selection events
         */
        public UnityEvent<UnitController> selectionEvent;
        public UnityEvent deselectionEvent;
        /*
         * Command events
         */
        public UnityEvent<UnitController> setTarget;
        public UnityEvent<Vector3> setDestination;
        public UnityEvent<ResourceController> setResource;
        
        /*
         * Ray-casting
         */
        private Ray _ray;
        private RaycastHit _raycastHit;
    
        /*
         * Input actions
         */
        private InputActions _inputActions;
        private InputAction _mousePosition;
        private InputAction _select;
        private InputAction _execute;

        private void Awake()
        {
            /*
             * Selection events
             */
            selectionEvent = new UnityEvent<UnitController>();
            deselectionEvent = new UnityEvent();
            
            /*
             * Command events
             */

            _inputActions = new InputActions();
            /*
             * Selection inputs
             */
            _mousePosition = _inputActions.UnitActionMap.MousePosition;
            _select = _inputActions.UnitActionMap.Select;
            
            /*
             * Command inputs
             */
            _execute = _inputActions.UnitActionMap.Execute;
        }
        private void OnEnable()
        {
            _select.started += _ => DraggingStarts();
            _select.performed += _ => SelectSingle();
            _select.canceled += _ => DraggingStops();
            _mousePosition.performed += _ => Dragging();
            _inputActions.Enable();

            _execute.performed += _ => RightClick();
        }
        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void RightClick()
        {
            if (IsMouseOverUi) return; // TODO debug it because doesnt work well

            Debug.Log("Right click!");
            // TODO check what we click on
        }
    
        private void SelectSingle()
        {
            if (IsMouseOverUi) return;

            if (Camera.main != null)
            {
                _ray = Camera.main.ScreenPointToRay(GetMousePosition());
            }
            /*
             * Deselect all
             */
            _DeselectAllUnits();
            _DeselectAllBuilding();
            deselectionEvent.Invoke();
            
            /*
             * Check if we can select anything
             */
            if (Physics.Raycast(_ray, out _raycastHit, 1000f))
            {
                if (_raycastHit.transform.CompareTag("Building"))
                {
                    _raycastHit.transform.gameObject.GetComponent<BuildingController>().Select();
                    selectionEvent.Invoke(_raycastHit.transform.gameObject.GetComponent<BuildingController>());
                }
                else if (_raycastHit.transform.CompareTag("Character"))
                {
                    _raycastHit.transform.gameObject.GetComponent<CharacterController>().Select();
                    selectionEvent.Invoke(_raycastHit.transform.gameObject.GetComponent<CharacterController>());
                }
                else if (_raycastHit.transform.CompareTag("Resource"))
                {
                    // TODO resource?
                }
            }
        }
        
        /*
         * Dragging selection
         */
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
            if (Camera.main == null) return;
            Bounds selectionBounds = SelectionBoundingBoxDrawer.GetViewportBounds(Camera.main, _dragStartPosition, GetMousePosition());

            /*
             * Can only select Characters this way
             */
            foreach (var characterEntry in GameManager.MY_CHARACTERS)
            {
                var inBounds = selectionBounds.Contains(Camera.main.WorldToViewportPoint(characterEntry.Value.gameObject.transform.position));
                var controller = characterEntry.Value.gameObject.GetComponent<UnitController>();
                if (inBounds)
                {
                    controller.Select();
                    if (GameManager.MY_SELECTED_CHARACTERS.Contains( (CharacterController)_raycastHit.transform.gameObject.GetComponent<UnitController>()))
                    {
                        selectionEvent.Invoke(_raycastHit.transform.gameObject.GetComponent<UnitController>());
                    }
                }
                else
                {
                    controller.Deselect();
                }
            }
        }
        /*
         * /Dragging selection
         */
        
        /*
         * Deselection of everything
         */
        private void _DeselectAllUnits()
        {
            List<CharacterController> selectedUnits = new List<CharacterController>(GameManager.MY_SELECTED_CHARACTERS);
            foreach (CharacterController usc in selectedUnits)
                usc.Deselect();
        }
        private void _DeselectAllBuilding()
        {
            List<BuildingController> selectedBuildings = new List<BuildingController>(GameManager.MY_SELECTED_BUILDINGS);
            foreach (BuildingController bsc in selectedBuildings)
                bsc.Deselect();
        }
        /*
         * /Deselection of everything
         */
       
        /*
         * Helper functions
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
        /*
         * /Helper functions
         */
        
        /*
        * Necessary to prevent rayCasting through UI
        */
        private bool IsMouseOverUi
        {
            get
            {
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
    }
}