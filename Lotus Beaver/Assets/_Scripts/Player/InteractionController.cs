using UnityEngine;
using UnityEngine.InputSystem;
public class InteractionController : MonoBehaviour
{
    [SerializeField] private InteractionSettings _interactionSettings;
    public Tile CurrentTile { get; private set; }
    private SpriteRenderer _currentSpriteRenderer;
    private Color _originalColor;
    private TileManager _tileManager;
    private Vector2 _moveInput;
    private Vector3 _interactionOffset;
    private void Start()
    {
        RefManager.inputManager.OnInteract += OnInteract;
        RefManager.inputManager.OnMove += OnMove;
    }
    private void Update()
    {
        Tile tile = TileManager.GetClosetTile(transform.position + _interactionOffset);
        if (tile != CurrentTile)
        {
            if (_currentSpriteRenderer != null)
            {
                _currentSpriteRenderer.color = _originalColor;
            }
            CurrentTile = tile;
            SpriteRenderer spriteRenderer = CurrentTile.TileElement.GetComponent<SpriteRenderer>();
            _originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            _currentSpriteRenderer = spriteRenderer;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (CurrentTile != null)
            {
                Debug.Log("Interact");
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            if (input != Vector2.zero)
            {
                _moveInput = input;
                _interactionOffset = new Vector3(_moveInput.x, _moveInput.y, 0) * _interactionSettings.InteractionDistance;
            }
        }
    }

    private void OnDrawGizmos() {
        if(_interactionSettings != null && _interactionSettings.DrawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _interactionOffset);
        }
    }
    private void OnDestroy()
    {
        RefManager.inputManager.OnInteract -= OnInteract;
        RefManager.inputManager.OnMove -= OnMove;
    }
}