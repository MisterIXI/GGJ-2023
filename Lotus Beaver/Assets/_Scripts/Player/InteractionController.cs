using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class InteractionController : MonoBehaviour
{
    private IInteractable[] Interactions;
    public static event Action<Tile> OnTileInteract;
    public static event Action<Tile> OnTileSelectionChange;
    public static event Action<int> OnInteractionChange;
    [SerializeField] private InteractionSettings _interactionSettings;
    public Tile CurrentTile { get; private set; }
    private SpriteRenderer _currentSpriteRenderer;
    private Color _originalColor;
    private TileManager _tileManager;
    private Vector2 _moveInput;
    private Vector3 _interactionOffset;
    private IInteractable _currentInteraction;
    private int _currentInteractionIndex;
    private void Start()
    {
        RefManager.inputManager.OnInteract += OnInteract;
        RefManager.inputManager.OnMove += OnMove;
        RefManager.inputManager.OnNextBuilding += OnNextBuilding;
        RefManager.inputManager.OnPrevBuilding += OnPreviousBuilding;
        RefManager.inputManager.OnBuildingKey += OnBuildingKey;
        InitializeInteractions();
        SelectInteraction(0);
    }
    private void Update()
    {
        TileSelectionUpdate();
    }

    private void InitializeInteractions()
    {
        Interactions = new IInteractable[2 + _interactionSettings.BuildingSettings.Length];
        Interactions[0] = new EarthInteraction();
        Interactions[1] = new WaterInteraction();
        for (int i = 0; i < _interactionSettings.BuildingSettings.Length; i++)
        {
            Interactions[i + 2] = new BuildingInteraction(_interactionSettings.BuildingSettings[i]);
        }
    }
    private void SelectInteraction(int index)
    {
        if (Interactions.Length > index)
        {
            Debug.Log("Interaction changed: " + _currentInteraction?.GetType().Name + " -> " + Interactions[index].GetType().Name);
            _currentInteraction = Interactions[index];
            _currentInteractionIndex = index;
            OnInteractionChange?.Invoke(_currentInteractionIndex);
        }
    }
    private void TileSelectionUpdate()
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
            OnTileSelectionChange?.Invoke(CurrentTile);
            _currentInteraction?.OnSelection(CurrentTile);
        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (CurrentTile != null)
            {
                OnTileInteract?.Invoke(CurrentTile);
                _currentInteraction?.OnInteract(CurrentTile);
                CurrentTile = null;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();

                _moveInput = input;
                // if(_moveInput.y > _moveInput.x)
                //     _moveInput.x = 0;
                // else
                //     _moveInput.y = 0;
                _interactionOffset = new Vector3(_moveInput.x, _moveInput.y, 0) * _interactionSettings.InteractionDistance;
        }
        if (context.canceled)
        {
            _moveInput = Vector2.zero;
            _interactionOffset = Vector3.zero;
        }
    }


    public void OnNextBuilding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SelectInteraction((_currentInteractionIndex + 1) % Interactions.Length);
        }
    }

    public void OnPreviousBuilding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_currentInteractionIndex == 0)
                _currentInteractionIndex = Interactions.Length - 1;
            else
                _currentInteractionIndex -= 1;
            SelectInteraction(_currentInteractionIndex);
        }
    }

    public void OnBuildingKey(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // try parse the name of the control to an int
            int index = 1;
            int.TryParse(context.control.name, out index);
            SelectInteraction(index - 1);
        }
    }

    private void OnDrawGizmos()
    {
        if (_interactionSettings != null && _interactionSettings.DrawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _interactionOffset);
        }
    }
    private void OnDestroy()
    {
        RefManager.inputManager.OnInteract -= OnInteract;
        RefManager.inputManager.OnMove -= OnMove;
        RefManager.inputManager.OnNextBuilding -= OnNextBuilding;
        RefManager.inputManager.OnPrevBuilding -= OnPreviousBuilding;
        RefManager.inputManager.OnBuildingKey -= OnBuildingKey;
    }
}