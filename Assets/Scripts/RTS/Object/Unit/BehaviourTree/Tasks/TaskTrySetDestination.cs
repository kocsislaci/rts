using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using CharacterController = RTS.Object.Unit.Character.CharacterController;

namespace RTS.Object.Unit.BehaviourTree.Tasks
{
    public class TaskTrySetDestination : Node
    {
        private CharacterController _controller;
        
        private Ray _ray;
        private RaycastHit _raycastHit;

        private InputActions _inputActions;
        private InputAction _execute;
        private InputAction _mousePosition;
        private InputAction _modifier; // shift to add to queue not to overwrite it

        public TaskTrySetDestination(ref CharacterController controller) : base()
        {
            _controller = controller;
            
            _inputActions = new InputActions();
            _execute = _inputActions.UnitActionMap.Execute;
            _mousePosition = _inputActions.UnitActionMap.MousePosition;
            _modifier = _inputActions.UnitActionMap.Modifier;
            _inputActions.Enable();
        }

        public override NodeState Evaluate()
        {
            if (_controller.IsSelected && _execute.WasPressedThisFrame() && !IsMouseOverUi)
            {
                _ray = Camera.main.ScreenPointToRay(GetMousePosition());
                if (Physics.Raycast(
                        _ray,
                        out _raycastHit,
                        1000f
                    ))
                {
                    _controller.SetDestination(_raycastHit.point);
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
                // Works with PhysicsRaycaster on the Camera.
                // Requires New Input System.
                // Assumes mouse.
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
