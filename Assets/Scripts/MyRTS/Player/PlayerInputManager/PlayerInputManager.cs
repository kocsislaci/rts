using System;
using System.Collections.Generic;
using MyRTS.GameManagers;
using MyRTS.Object.Resource;
using MyRTS.Object.Unit;
using MyRTS.Object.Unit.Building;
using MyRTS.Player.Commands;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using CharacterController = MyRTS.Object.Unit.Character.CharacterController;

namespace MyRTS.Player.PlayerInputManager
{
    [Serializable]
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        /*
        * Input actions
        */
        private InputActions _inputActions;
        
        private InputAction _mousePosition;
        private InputAction _select;
        private InputAction _execute;
        private InputAction _modifier;
        private InputAction _cancel;
        
        /*
         * Ray-casting
         */
        private Ray _ray;
        private RaycastHit _raycastHit;
        private LayerMask _uiLayerMask;
        private LayerMask _terrainLayerMask;
        private LayerMask _resourceLayerMask;
        private LayerMask _selectableLayerMask;
        private LayerMask _interactableLayerMask;
        private LayerMask _gameSpaceLayerMask;

        private const int TerrainLayer = 6;
        private const int ResourceLayer = 8;
        private const int UnitLayer = 9;
        private const int CharacterLayer = 10;
        private const int BuildingLayer = 11;

        
        private bool _isDraggingMouseBox = false;
        private Vector3? _dragStartPosition = null;
        private Vector3? _dragLastPosition = null;

        private float? _timeStamp;
        private const float DragRefreshTimePeriod = 0.2f;
        
        public UserInputState UserInputState
        {
            get => userInputState;

            set
            {
                userInputState = value;
            }
        }

        [SerializeField] private UserInputState userInputState = UserInputState.NothingSelected;
        

        public UnityEvent<UnityEngine.Object, UserInputState> LeftClick { get; } = new();
        public UnityEvent<UnityEngine.Object, UserInputState> LeftClickDragging { get; } = new();


        
        public UnityEvent<Vector3, UserInputState> RightClick { get; } = new();
        public UnityEvent<UserInputState, List<UnitType>> UserInputStateChange { get; } = new();
        

        private void Awake()
        {
            _inputActions = new InputActions();
            
            _mousePosition = _inputActions.UnitActionMap.MousePosition;
            _select = _inputActions.UnitActionMap.Select;
            _execute = _inputActions.UnitActionMap.Execute;
            _modifier = _inputActions.UnitActionMap.Modifier;
            _cancel = _inputActions.UnitActionMap.Cancel;
            
            _terrainLayerMask = LayerMask.GetMask("Terrain");
            _uiLayerMask = LayerMask.GetMask("UI");
            _resourceLayerMask = LayerMask.GetMask("Resource");
            _selectableLayerMask = LayerMask.GetMask("Character", "Building");
            _interactableLayerMask = LayerMask.GetMask("Resource", "Character", "Building");
            _gameSpaceLayerMask = LayerMask.GetMask("Terrain", "Resource", "Character", "Building");
        }
        private void OnEnable()
        {
            _select.started += _ => LeftClickStarted();
            _select.performed += _ => LeftClickPerformed();
            _select.canceled += _ => LeftClickCanceled();
            
            _mousePosition.performed += _ => MouseMoved();
            
            _execute.performed += _ => RightClickPerformed();
            _cancel.performed += _ => EscButtonPerformed();
                
            _inputActions.Enable();
        }
        private void OnDisable()
        {
            _inputActions.Disable();
        }
        
        public void InitialisePlayerInputManager(PlayerManager pPlayerManager)
        {
            playerManager = pPlayerManager;
        }
        
        
        /*
         * Implementation
         */
        private void EscButtonPerformed()
        {
            switch (UserInputState)
            {
                case UserInputState.NothingSelected:
                    break;
                case UserInputState.OneUnitTypeSelected:
                    DeselectAll();
                    break;
                case UserInputState.MultipleUnitTypesSelected:
                    DeselectAll();
                    break;
                case UserInputState.BuildActionSelected:
                    break; 
            }
        }

        private void RightClickPerformed()
        {
            if (IsMouseOverUi) return;
            if (playerManager.MySelectedUnits.Count == 0) return;

            var hit = CastRay(GetMousePosition(), _gameSpaceLayerMask);
            if (hit == null)
            {
                return;
            }

            // depending on the clicked something, we initialize the command
            CommandDto newCommand;
            switch (hit.Value.transform.gameObject.layer)
            {
                case TerrainLayer:
                    Debug.Log("clicked on terrain to set target destination or rallyPoint");
                    newCommand = new CommandDto(GameResources.MapManager.SampleHeightFromWorldPosition(hit.Value.point));
                    break;
                case ResourceLayer:
                    Debug.Log("clicked on resource to set resourceTarget");
                    newCommand = new CommandDto(null, null, hit.Value.transform.gameObject.GetComponent<ResourceController>());
                    break;
                case CharacterLayer:
                    Debug.Log("clicked on character to set followTarget or to try attack");
                    newCommand = new CommandDto(null, hit.Value.transform.gameObject.GetComponent<CharacterController>());
                    break;
                case BuildingLayer:
                    Debug.Log("clicked on building to set followTarget, if have harvested stuff then to load of, if buildable then build or to try attack");
                    newCommand = new CommandDto(null, hit.Value.transform.gameObject.GetComponent<BuildingController>());
                    break;
                default:
                    newCommand = null;
                    break;
            }

            
            // Go over on every selected entity
            foreach (var selectedUnitEntry in playerManager.MySelectedUnits)
            {
                // Send the command to the selected
                if (newCommand != null)
                {
                    selectedUnitEntry.Value.AddCommandToOverwrite(newCommand);
                }
            }
        }
        private void LeftClickPerformed()
        {
            if (IsMouseOverUi) return;

            DeselectAll();
            
            UserInputState = UserInputState.NothingSelected;
            UserInputStateChange.Invoke(UserInputState, new List<UnitType>() {});


            var hit = CastRay(GetMousePosition(), _selectableLayerMask);
            if (hit == null) return;
            var selected = (hit.Value.transform.gameObject).GetComponent<UnitController>();

            if (selected.Owner != playerManager) return;

            selected.Select();
            playerManager.MySelectedUnits.Add(selected.Uuid, selected);
            UserInputState = UserInputState.OneUnitTypeSelected;
            UserInputStateChange.Invoke(UserInputState, new List<UnitType>() {selected.Data.type});
        }

        
        /*
         * Dragging selection
         */
        private void LeftClickStarted()
        {
            if (IsMouseOverUi) return;

            _timeStamp = Time.time;
            _dragStartPosition = GetMousePosition();
            _dragLastPosition = _dragStartPosition;
            _isDraggingMouseBox = true;
        }
        private void MouseMoved()
        {
            if (IsMouseOverUi) return;
        
            if (_isDraggingMouseBox && 
                Vector3.Magnitude((Vector3)(_dragLastPosition - GetMousePosition())) >= 0.1f  &&
                (Time.time - _timeStamp) >= DragRefreshTimePeriod)
            {
                _SelectCharactersInDraggingBox();
                _timeStamp = Time.time;
            }
            _dragLastPosition = GetMousePosition();
        }
        private void LeftClickCanceled()
        {
            _isDraggingMouseBox = false;
            _dragStartPosition = null;
            _dragLastPosition = null;
            _timeStamp = null;
        }
        private void _SelectCharactersInDraggingBox()
        {
            if (Camera.main == null) return;
            if (_dragStartPosition == null) return;
            
            Bounds selectionBounds = SelectionBoundingBoxDrawer.GetViewportBounds(Camera.main, (Vector3)_dragStartPosition, GetMousePosition());
            
            foreach (var unitEntry in playerManager.MyUnits)
            {
                var selectable = unitEntry.Value.gameObject.GetComponent<UnitController>();
                if (selectionBounds.Contains(
                        Camera.main.WorldToViewportPoint(
                            (selectable).gameObject.transform.position)))
                {
                    if (!playerManager.MySelectedUnits.ContainsKey(selectable.Uuid))
                    {
                        selectable.Select();
                        playerManager.MySelectedUnits.Add(selectable.Uuid,selectable);
                    }
                }
                else
                {
                    if (playerManager.MySelectedUnits.ContainsKey(selectable.Uuid))
                    {
                        selectable.Deselect();
                        playerManager.MySelectedUnits.Remove(selectable.Uuid);
                    }
                }
                if (playerManager.MySelectedUnits.Count > 0)
                {
                    if (playerManager.MySelectedUnits.Count > 1)
                    {
                        var multipleTypeFound = false;
                        var typesFound = new List<Type>();
                        foreach (var unitControllerEntry in playerManager.MySelectedUnits)
                        {
                            var typeNowFound = unitControllerEntry.Value.GetType();
                            if (typesFound.Contains(typeNowFound))
                            {
                                multipleTypeFound = true;
                            }
                            else
                            {
                                typesFound.Add(typeNowFound);
                            }
                        }
                        if (multipleTypeFound)
                        {
                            UserInputState = UserInputState.MultipleUnitTypesSelected;
                            UserInputStateChange.Invoke(UserInputState, new List<UnitType>() {});
                        }
                        else
                        {
                            UserInputState = UserInputState.OneUnitTypeSelected;
                            UserInputStateChange.Invoke(UserInputState, new List<UnitType>() {});
                        }
                    }
                    else
                    {
                        UserInputState = UserInputState.OneUnitTypeSelected;
                        UserInputStateChange.Invoke(UserInputState, new List<UnitType>() {});
                    }
                }
                else
                {
                    UserInputState = UserInputState.NothingSelected;
                    UserInputStateChange.Invoke(UserInputState, new List<UnitType>() {});
                }
            }
        }
        
        /*
         * Deselection of everything
         */
        private void DeselectAll()
        {
            foreach (var playerManagerMySelectedUnit in playerManager.MySelectedUnits)
            {
                playerManagerMySelectedUnit.Value.Deselect();
            }
            playerManager.MySelectedUnits.Clear();
        }

        /*
         * Helper functions
         */
        private Vector3 GetMousePosition()
        {
            return new Vector3(
                _mousePosition.ReadValue<Vector2>().x,
                _mousePosition.ReadValue<Vector2>().y,
                0.0f);
        }
        private RaycastHit? CastRay(Vector3 mousePosition, LayerMask layerMask)
        {
            if (Camera.main != null)
            {
                _ray = Camera.main.ScreenPointToRay(mousePosition);
            }
            if (Physics.Raycast(_ray, out _raycastHit, 1000f, layerMask))
            {
                return _raycastHit;
            }
            return null;
        }
        private void OnGUI()
        {
            if (_isDraggingMouseBox)
            {
                var dragStartPosition = (Vector3)_dragStartPosition;
                var rect = SelectionBoundingBoxDrawer.GetScreenRect(dragStartPosition, GetMousePosition());
                
                SelectionBoundingBoxDrawer.DrawScreenRect(rect, new Color(0.5f, 1f, 0.4f, 0.2f));
                SelectionBoundingBoxDrawer.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
            }
        }
        private bool IsMouseOverUi 
        {
            get
            {
                {
                    RaycastResult lastRaycastResult = ((InputSystemUIInputModule)EventSystem.current.currentInputModule).GetLastRaycastResult(Mouse.current.deviceId);
                    return lastRaycastResult.gameObject != null;
                }
            }
        }
    }
}