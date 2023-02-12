using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private MovementSettings _movementSettings;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [field: SerializeField] public Transform CameraTarget { get; private set; }

    private Vector3 _startPosition;
    private Animator animator;
    private Vector2 _moveInput;
    private Vector2 _movement;
    private Rigidbody2D _rigidbody;
    private enum FadeState
    { none, fadein, fadeout };

    private FadeState _fadeState = FadeState.none;
    private float _fadeStartTime;

    private Transform _transform;

    private readonly int _horizontal = Animator.StringToHash("Horizontal");
    private readonly int _vertical = Animator.StringToHash("Vertical");

    private void Awake()
    {
        _transform = transform;

        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        InputManager.OnMove += OnMoveInput;
        InputManager.OnReset += OnResetInput;

        _startPosition = _transform.position;
    }

    private void Update()
    {
        HandleMovement();
        HandleCameraTargetMovement();
        HandleFade();
        Tile tile = TileManager.GetClosetTile(_transform.position);
        UpdateSortOrder(tile);

        if (_movementSettings.resetOnWater)
        {
            if (tile != null && (tile.TileElement?.TileElementType == TileElementType.Water || TileManager.IsOutOfBounds(tile)))
            {
                StartReset();
            }
        }

        animator.SetFloat(_horizontal, _movement.x);
        animator.SetFloat(_vertical, _movement.y);
    }

    private void UpdateSortOrder(Tile tile)
    {
        if (tile == null)
        {
            return;
        }

        _spriteRenderer.sortingOrder = TileManager.GetSortOrderFromCoordinate(tile.Coordinates, 2);
    }

    private void HandleMovement()
    {
        _movement = Vector2.MoveTowards(_movement, _moveInput, _movementSettings.acceleration * Time.deltaTime);
        _rigidbody.velocity = _fadeState != FadeState.none ? Vector2.zero : _movement * _movementSettings.moveSpeed;
    }

    private void HandleCameraTargetMovement()
    {
        CameraTarget.position = _transform.position + (new Vector3(_movement.x, _movement.y, -_movementSettings.cameraZDistance) * _movementSettings.cameraDistance);
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
        switch (_fadeState)
        {
            case FadeState.fadeout:
                {
                    float t = (Time.time - _fadeStartTime) / _movementSettings.fadeDuration;
                    HeadUpDisplayActiveElements.FadeImage.color = Color.Lerp(Color.clear, Color.black, t);
                    if (t >= 1)
                    {
                        _fadeState = FadeState.fadein;
                        _transform.position = _startPosition;
                        _fadeStartTime = Time.time;
                    }

                    break;
                }

            case FadeState.fadein:
                {
                    float t = (Time.time - _fadeStartTime) / _movementSettings.fadeDuration;
                    HeadUpDisplayActiveElements.FadeImage.color = Color.Lerp(Color.black, Color.clear, t);
                    if (t >= 1)
                    {
                        _fadeState = FadeState.none;
                    }

                    break;
                }

            default:
                {
                    break;
                }
        }
    }

    private void StartReset()
    {
        if (_fadeState == FadeState.none)
        {
#if UNITY_EDITOR
            Debug.Log("Resetting Player...");
#endif
            _fadeStartTime = Time.time;
            _fadeState = FadeState.fadeout;
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
        InputManager.OnMove -= OnMoveInput;
        InputManager.OnReset -= OnResetInput;
    }
}