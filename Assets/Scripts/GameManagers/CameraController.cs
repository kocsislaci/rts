using System;
using System.Collections;
using System.Collections.Generic;
using Terrain;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    private InputActions _cameraActions;
    private InputAction _movement;
    private InputAction _mousePosition;
    private Camera _camera;
    private Transform _cameraTransform;
    private Mouse _mouse;

    private UnityEngine.Terrain _terrain;

    // Horizontal translation
    [SerializeField] private float _maxSpeed = 5f; 
    private float _speed;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _damping = 15f;    
    private float _xMax = 1000f;
    private float _xMin = 0f;
    private float _zMax = 1000f;
    private float _zMin = 0f;
    // Vertical translation
    [SerializeField] private float _stepSize = 2f;
    [SerializeField] private float _zoomDampening = 7.5f;
    [SerializeField] private float _minHeight = 5f;
    [SerializeField] private float _maxHeight = 50f;
    [SerializeField] private float _zoomSpeed = 2f;
    
    // Rotation
    [SerializeField] private float _maxRotationSpeed = 2f;

    [SerializeField] [Range(0f, 0.1f)] private float _edgeTolerance = 0.05f;

    private Vector3 _targetPosition;
    
    private float _zoomHeight;

    private Vector3 _horizontalVelocity;
    private Vector3 _lastPosition;

    private Vector3 _startDrag;
    
    private void Awake()
    {
        _cameraActions = new InputActions();
        _camera = this.GetComponentInChildren<Camera>();
        _cameraTransform = _camera.transform;
        _mouse = Mouse.current;
    }

    private void Start()
    {
        _terrain = GameObject.FindWithTag("Terrain").GetComponent<UnityEngine.Terrain>();
        UpdateBasePosition();
    }

    private void OnEnable()
    {
        _zoomHeight = _cameraTransform.localPosition.y;
        _cameraTransform.LookAt(this.transform);

        _lastPosition = this.transform.position;

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
        // CheckMouseAtScreenEdge();
        //DragCamera();
        
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

        if (mousePosition.x < _edgeTolerance * Screen.width)
            moveDirection += -GetCameraRight();
        else if (mousePosition.x > (1f - _edgeTolerance) * Screen.width)
            moveDirection += GetCameraRight();
        
        if (mousePosition.y < _edgeTolerance * Screen.height)
            moveDirection += -GetCameraForward();
        else if (mousePosition.y > (1f - _edgeTolerance) * Screen.height)
            moveDirection += GetCameraForward();

        _targetPosition += moveDirection;
    }
    private void DragCamera()
    {
        if (!_mouse.rightButton.isPressed)
            return;

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = _camera.ScreenPointToRay(_mousePosition.ReadValue<Vector2>());

        if (plane.Raycast(ray, out float distance))
        {
            if (_mouse.rightButton.wasPressedThisFrame)
                _startDrag = ray.GetPoint(distance);
            else
                _targetPosition += _startDrag - ray.GetPoint(distance);
        }
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
        var rigPosition = this.transform.position;
        _horizontalVelocity = (rigPosition - _lastPosition) / Time.deltaTime;
        _horizontalVelocity.y = 0f;
        _lastPosition = rigPosition;
    }
    private void UpdateBasePosition()
    {
        if (_targetPosition.sqrMagnitude > 0.1f)
        {
            _speed = Mathf.Lerp(_speed, _maxSpeed,  _acceleration * Time.deltaTime);
            transform.position += _targetPosition * (_speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x ,_terrain.SampleHeight(transform.position), transform.position.z);
            if (transform.position.x < _xMin)
                transform.position = new Vector3(_xMin, transform.position.y, transform.position.z);
            if (transform.position.x > _xMax)
                transform.position = new Vector3(_xMax, transform.position.y, transform.position.z);
            if (transform.position.z < _zMin)
                transform.position = new Vector3(transform.position.x, transform.position.y, _zMin);
            if (transform.position.z > _zMax)
                transform.position = new Vector3(transform.position.x, transform.position.y, _zMax);
        }
        else
        {
            _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, _damping * Time.deltaTime);
            transform.position += _horizontalVelocity * Time.deltaTime;
        }
        
        _targetPosition = Vector3.zero;
    }
    
    private void RotateCamera(InputAction.CallbackContext obj)
    {
        if (!_mouse.middleButton.isPressed)
            return;

        float inputValue = obj.ReadValue<Vector2>().x;
        float nextRotationValue = inputValue * _maxRotationSpeed + transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, nextRotationValue, 0f);
    }
    
    private void ZoomCamera(InputAction.CallbackContext obj)
    {
        float inputValue = -obj.ReadValue<Vector2>().y / 100.0f;

        if (Math.Abs(inputValue) > 0.1f)
        {
            _zoomHeight = _cameraTransform.localPosition.y + inputValue * _stepSize;

            if (_zoomHeight < _minHeight)
                _zoomHeight = _minHeight;
            else if (_zoomHeight > _maxHeight)
                _zoomHeight = _maxHeight;
        }
    }
    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(_cameraTransform.localPosition.x, 
            _zoomHeight, 
            _cameraTransform.localPosition.z);
        zoomTarget -= _zoomSpeed * (_zoomHeight - _cameraTransform.localPosition.y) * Vector3.forward;

        _cameraTransform.localPosition =
            Vector3.Lerp(_cameraTransform.localPosition, zoomTarget, _zoomDampening * Time.deltaTime);
        _cameraTransform.LookAt(this.transform);
    }
}
