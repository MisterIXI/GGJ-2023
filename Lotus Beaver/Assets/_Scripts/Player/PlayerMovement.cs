using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    private HeadUpDisplayActiveElements _headUpDisplayActiveElements;
    private Image _fadeImage;
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
        RefManager.inputManager.OnReset += OnResetInput;
        _startPosition = transform.position;
        _headUpDisplayActiveElements = HeadUpDisplayElement.Instance.GetComponentInChildren<HeadUpDisplayActiveElements>();
        _fadeImage = _headUpDisplayActiveElements.FadeImage;
    }

    private void Update()
    {
        HandleMovement();
        HandleCameraTargetMovement();
        HandleFade();
        animator.SetFloat("Horizontal", _movement.x);
        animator.SetFloat("Vertical", _movement.y);
        Tile tile = TileManager.GetClosetTile(transform.position);
        if (tile.TileElement?.TileElementType == TileElementType.Water)
        {
            StartReset();
        }

    }

    private void HandleMovement()
    {
        _movement = Vector2.MoveTowards(_movement, _moveInput, _movementSettings.acceleration * 2 * 10 * Time.deltaTime);
        if (_fadeState != fadeState.none)
            _rigidbody.velocity = Vector2.zero;
        else
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
        if (_fadeState == fadeState.fadeout)
        {
            float t = (Time.time - _fadeStartTime) / _movementSettings.fadeDuration;
            _fadeImage.color = Color.Lerp(Color.clear, Color.black, t);
            if (t >= 1)
            {
                _fadeState = fadeState.fadein;
                transform.position = _startPosition;
                _fadeStartTime = Time.time;
            }
        }
        else if (_fadeState == fadeState.fadein)
        {
            float t = (Time.time - _fadeStartTime) / _movementSettings.fadeDuration;
            _fadeImage.color = Color.Lerp(Color.black, Color.clear, t);
            if (t >= 1)
            {
                _fadeState = fadeState.none;
            }
        }
    }

    private void StartReset()
    {
        if (_fadeState == fadeState.none)
        {
            Debug.Log("Resetting Player...");
            _fadeStartTime = Time.time;
            _fadeState = fadeState.fadeout;
        }
    }
    public void OnResetInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartReset();
        }
    }

    private void OnDestroy()
    {
        RefManager.inputManager.OnMove -= OnMoveInput;
        RefManager.inputManager.OnReset -= OnResetInput;
    }
}
