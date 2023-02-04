using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Transform CameraTarget { get; private set; }
    private Vector3 _startPosition;
    private Animator animator;
    private Vector2 _moveInput;
    private Vector2 _movement;
    private Rigidbody2D _rigidbody;
    [SerializeField] private MovementSettings _movementSettings;

    private enum fadeState { none, fadein, fadeout };
    private fadeState _fadeState = fadeState.none;
    private float _fadeStartTime;
    private void Awake()
    {
        CameraTarget = new GameObject("CameraTarget").transform;
        CameraTarget.SetParent(transform);

        animator = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        RefManager.inputManager.OnMove += OnMoveInput;
        _startPosition = transform.position;
    }

    private void Update()
    {
        HandleMovement();
        HandleCameraTargetMovement();
        HandleFade();
        animator.SetFloat("Horizontal", _movement.x);
        animator.SetFloat("Vertical", _movement.y);
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

    private void HandleFade()
    {
        if(_fadeState == fadeState.fadeout)
        {
            
        }
    }
    public void OnResetInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_fadeState == fadeState.none)
            {
                _fadeStartTime = Time.time;
                _fadeState = fadeState.fadein;
            }
        }
    }

    private void OnDestroy()
    {
        RefManager.inputManager.OnMove -= OnMoveInput;
    }
}
