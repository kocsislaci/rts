using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using CharacterController = Unit.Character.CharacterController;


namespace Unit.BehaviourTree.Tasks
{
    public class TaskTrySetDestinationOrTarget: Node
    {
        CharacterController _controller;

        private Ray _ray;
        private RaycastHit _raycastHit;
        
        private InputActions _inputActions;
        private InputAction _execute;
        private InputAction _mousePosition;

        public TaskTrySetDestinationOrTarget(CharacterController controller) : base()
        {
            _controller = controller;
            
            _inputActions = new InputActions();
            _mousePosition = _inputActions.UnitActionMap.MousePosition;
            _execute = _inputActions.UnitActionMap.Execute;
            _inputActions.Enable();
        }

        public override NodeState Evaluate()
        {
            if (_controller.isSelected && _execute.WasPressedThisFrame() && !IsMouseOverUi)
            {
                _ray = Camera.main.ScreenPointToRay(GetMousePosition());
                if (Physics.Raycast(
                        _ray,
                        out _raycastHit,
                        1000f
                    ))
                {
                    UnitController uc = _raycastHit.collider.GetComponent<UnitController>();
                    if (uc != null)
                    {
                        Parent.Parent.SetData("currentTarget", _raycastHit.transform);
                        ClearData("destinationPoint");
                        _state = NodeState.SUCCESS;
                        return _state;
                    }
                }

                else if (Physics.Raycast(
                             _ray,
                             out _raycastHit,
                             1000f
                         ))
                {
                    ClearData("currentTarget");
                    Parent.Parent.SetData("destinationPoint", _raycastHit.point);
                    _state = NodeState.SUCCESS;
                    return _state;
                }
            }
            _state = NodeState.FAILURE;
            return _state;
        }
        private Vector3 GetMousePosition()
        {
            return new Vector3(
                _mousePosition.ReadValue<Vector2>().x,
                _mousePosition.ReadValue<Vector2>().y,
                0.0f);
        }
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
