using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RTS.RTSCamera
{
    public class CameraController : MonoBehaviour
    {
        private InputActions _cameraActions;
        private InputAction _movement;
        private InputAction _mousePosition;
        private Camera _camera;
        private Transform _cameraTransform;
        private Mouse _mouse;
        private UnityEngine.Terrain _terrain;

        /*
         * Horizontal translation
         */
        [SerializeField] private float maxSpeed = 5f; 
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float damping = 15f;
        [SerializeField] [Range(0f, 0.1f)] private float edgeTolerance = 0.05f;
        [SerializeField] private bool checkMouseAtScreenEdge = false;
        private float _speed;
        private const float XMax = 512;
        private const float XMin = 0f;
        private const float ZMax = 512;
        private const float ZMin = 0f;


        /*
         * Vertical translation
         */
        [SerializeField] private float stepSize = 2f;
        [SerializeField] private float zoomDampening = 7.5f;
        [SerializeField] private float minHeight = 5f;
        [SerializeField] private float maxHeight = 50f;
        [SerializeField] private float zoomSpeed = 2f;
    
        /*
         * Rotation
         */
        [SerializeField] private float maxRotationSpeed = 2f;


        private Vector3 _targetPosition;
        private float _zoomHeight;
        private Vector3 _horizontalVelocity;
        private Vector3 _lastPosition;

        public void SetStartPosition(Vector3 position)
        {
            transform.position = position;
            
            _zoomHeight = _cameraTransform.localPosition.y;
            _cameraTransform.LookAt(transform);

            _lastPosition = transform.position;
        }
        public void FindAndSetTerrain()
        {
            _terrain = GameObject.FindWithTag("Terrain").GetComponent<UnityEngine.Terrain>();
        }
        
        private void Awake()
        {
            FindAndSetTerrain();
                
            _cameraActions = new InputActions();
            _camera = GetComponentInChildren<UnityEngine.Camera>();
            _cameraTransform = _camera.transform;
            _mouse = Mouse.current;
            
            UpdateVelocity();
            UpdateBasePosition();
            UpdateCameraPosition();
        }

        private void OnEnable()
        {
            _zoomHeight = _cameraTransform.localPosition.y;
            _cameraTransform.LookAt(this.transform);

            _lastPosition = transform.position;

            _movement = _cameraActions.CameraMovementActionMap.MoveCamera;
            _mousePosition = _cameraActions.CameraMovementActionMap.MousePosition;
            _cameraActions.CameraMovementActionMap.RotateCamera.performed += RotateCamera;
            _cameraActions.CameraMovementActionMap.ZoomCamera.performed += ZoomCamera;
            _cameraActions.CameraMovementActionMap.Enable();
        }
        private void OnDisable()
        {
            _cameraActions.CameraMovementActionMap.RotateCamera.performed -= RotateCamera;
            _cameraActions.CameraMovementActionMap.ZoomCamera.performed -= ZoomCamera;
            _cameraActions.CameraMovementActionMap.Disable();
        }

        private void Update()
        {
            GetKeyboardMovement();
            if (checkMouseAtScreenEdge) CheckMouseAtScreenEdge();
        
            UpdateVelocity();
            UpdateBasePosition();
            UpdateCameraPosition();
        }

        private void GetKeyboardMovement()
        {
            Vector3 inputValue = _movement.ReadValue<Vector2>().x * GetCameraRight() +
                                 _movement.ReadValue<Vector2>().y * GetCameraForward();
            inputValue = inputValue.normalized;

            if (inputValue.sqrMagnitude > 0.1f)
            {
                _targetPosition += inputValue;
            }
        }
        private void CheckMouseAtScreenEdge()
        {
            Vector2 mousePosition = _mousePosition.ReadValue<Vector2>();
            Vector3 moveDirection = Vector3.zero;

            if (mousePosition.x < edgeTolerance * Screen.width)
                moveDirection += -GetCameraRight();
            else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
                moveDirection += GetCameraRight();
        
            if (mousePosition.y < edgeTolerance * Screen.height)
                moveDirection += -GetCameraForward();
            else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
                moveDirection += GetCameraForward();

            _targetPosition += moveDirection;
        }
        private Vector3 GetCameraForward()
        {
            Vector3 forward = _cameraTransform.forward;
            forward.y = 0f;
            return forward;
        }
        private Vector3 GetCameraRight()
        {
            Vector3 right = _cameraTransform.right;
            right.y = 0f;
            return right;
        }
        private void UpdateVelocity()
        {
            var rigPosition = transform.position;
            _horizontalVelocity = (rigPosition - _lastPosition) / Time.deltaTime;
            _horizontalVelocity.y = 0f;
            _lastPosition = rigPosition;
        }
        private void UpdateBasePosition()
        {
            if (_targetPosition.sqrMagnitude > 0.1f)
            {
                _speed = Mathf.Lerp(_speed, maxSpeed,  acceleration * Time.deltaTime);
                transform.position += _targetPosition * (_speed * Time.deltaTime);
                transform.position = new Vector3(transform.position.x ,_terrain.SampleHeight(transform.position), transform.position.z);
                if (transform.position.x < XMin)
                    transform.position = new Vector3(XMin, transform.position.y, transform.position.z);
                if (transform.position.x > XMax)
                    transform.position = new Vector3(XMax, transform.position.y, transform.position.z);
                if (transform.position.z < ZMin)
                    transform.position = new Vector3(transform.position.x, transform.position.y, ZMin);
                if (transform.position.z > ZMax)
                    transform.position = new Vector3(transform.position.x, transform.position.y, ZMax);
            }
            else
            {
                _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, damping * Time.deltaTime);
                transform.position += _horizontalVelocity * Time.deltaTime;
            }
        
            _targetPosition = Vector3.zero;
        }
        private void RotateCamera(InputAction.CallbackContext obj)
        {
            if (!_mouse.middleButton.isPressed)
                return;

            float inputValue = obj.ReadValue<Vector2>().x;
            float nextRotationValue = inputValue * maxRotationSpeed + transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, nextRotationValue, 0f);
        }
        private void ZoomCamera(InputAction.CallbackContext obj)
        {
            float inputValue = -obj.ReadValue<Vector2>().y / 100.0f;

            if (Math.Abs(inputValue) > 0.1f)
            {
                _zoomHeight = _cameraTransform.localPosition.y + inputValue * stepSize;

                if (_zoomHeight < minHeight)
                    _zoomHeight = minHeight;
                else if (_zoomHeight > maxHeight)
                    _zoomHeight = maxHeight;
            }
        }
        private void UpdateCameraPosition()
        {
            Vector3 zoomTarget = new Vector3(_cameraTransform.localPosition.x, 
                _zoomHeight, 
                _cameraTransform.localPosition.z);
            zoomTarget -= zoomSpeed * (_zoomHeight - _cameraTransform.localPosition.y) * Vector3.forward;

            _cameraTransform.localPosition =
                Vector3.Lerp(_cameraTransform.localPosition, zoomTarget, zoomDampening * Time.deltaTime);
            _cameraTransform.LookAt(this.transform);
        }
    }
}
