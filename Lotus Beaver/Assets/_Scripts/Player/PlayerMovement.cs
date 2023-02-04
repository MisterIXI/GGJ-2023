using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Transform CameraTarget { get; private set; }
    private Vector2 _moveInput;
    private Vector2 _movement;
    private Rigidbody2D _rigidbody;
    [SerializeField] private MovementSettings _movementSettings;
    private void Awake()
    {
        CameraTarget = new GameObject("CameraTarget").transform;
        CameraTarget.SetParent(transform);
    }
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        RefManager.inputManager.OnMove += OnMoveInput;
    }

    private void Update()
    {
        HandleMovement();
        HandleCameraTargetMovement();
    }

    private void HandleMovement()
    {
        _movement = Vector2.MoveTowards(_movement, _moveInput, _movementSettings.acceleration * 2 * 10 * Time.deltaTime);
        _rigidbody.velocity = _movement * _movementSettings.moveSpeed;
    }

    private void HandleCameraTargetMovement()
    {
        CameraTarget.position = transform.position + new Vector3(_movement.x, _movement.y, -_movementSettings.cameraZDistance) * _movementSettings.cameraDistance;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _moveInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            _moveInput = Vector2.zero;
        }
    }

    private void OnDestroy()
    {
        RefManager.inputManager.OnMove -= OnMoveInput;
    }
}
