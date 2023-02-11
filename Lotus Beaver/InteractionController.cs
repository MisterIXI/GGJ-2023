using System;
using System.Data;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private InteractionSettings _interactionSettings;
    [field: SerializeField] public InteractionPreview InteractionPreview { get; private set; }

    [field: SerializeField] private Color greenColor;
    [field: SerializeField] private Color redColor;
    [field: SerializeField] private Color previewBorderRedColor;
    [field: SerializeField] private Color previewBorderGreenColor;

    private GameSettings _gameSettings;
    private IInteractable[] Interactions;

    private Vector2 _moveInput;
    private Vector3 _interactionOffset;
    private IInteractable _currentInteraction;
    private HeadUpDisplayActiveElements _hud;
    private int _currentInteractionIndex;
    private TextMeshProUGUI _interactionText;
    private TextMeshProUGUI _waterCostText;
    private TextMeshProUGUI _earthCostText;
    private TextMeshProUGUI _interactionDescriptionText;

    public SpriteRenderer BuildPreviewSpriteRenderer { get; private set; }
    public SpriteRenderer InteractionPreviewSpriteRenderer { get; private set; }
    public InteractableBase CurrentBuilding { get; private set; }
    public Tile CurrentTile { get; private set; }

    public static event Action<Tile> OnTileInteract;

    public static event Action<Tile> OnTileSelectionChange;

    public static event Action<int> OnInteractionChange;

    private void Awake()
    {
        InputManager.OnInteract += OnInteract;
        InputManager.OnMove += OnMove;
        InputManager.OnNextBuilding += OnNextBuilding;
        InputManager.OnPrevBuilding += OnPreviousBuilding;
        InputManager.OnBuildingKey += OnBuildingKey;
    }

    private void Start()
    {
        InitializeInteractions();
        SelectInteraction(0);
        BuildPreviewSpriteRenderer = InteractionPreview.transform.GetChild(0).GetComponent<SpriteRenderer>();
        InteractionPreviewSpriteRenderer = InteractionPreview.GetComponent<SpriteRenderer>();
        _gameSettings = GameSettingsManager.GameSettings();
        _hud = HeadUpDisplayActiveElements.Instance;
        _interactionText = _hud.InteractionText;
        _waterCostText = _hud.CostWaterText;
        _earthCostText = _hud.CostEarthText;
        _interactionDescriptionText = _hud.InteractionDescriptionText;
    }

    private void Update()
    {
        TileSelectionUpdate();
        UpdatePreview();
    }

    private void InitializeInteractions()
    {
        Interactions = new IInteractable[1 + _interactionSettings.BuildingSettings.Length];
        Interactions[0] = new EarthInteraction(this, 0);
        for (int i = 0; i < _interactionSettings.BuildingSettings.Length; i++)
        {
            Interactions[i + 1] = new BuildingInteraction(_interactionSettings.BuildingSettings[i], this, i + 1);
        }
    }

    private void SelectInteraction(int index)
    {
        if (Interactions.Length > index)
        {
#if UNITY_EDITOR
            Debug.Log("Interaction changed: " + _currentInteraction?.GetType().Name + " -> " + Interactions[index].GetType().Name);
#endif
            _currentInteraction = Interactions[index];
            _currentInteractionIndex = index;
            OnInteractionChange?.Invoke(_currentInteractionIndex);
            SoundManager.PlaySelectInteraction();
        }
    }

    private void TileSelectionUpdate()
    {
        Tile tile = TileManager.GetClosetTile(transform.position + _interactionOffset);
        if (tile != CurrentTile)
        {
            CurrentTile = tile;
            OnTileSelectionChange?.Invoke(CurrentTile);
            _currentInteraction?.OnSelection(CurrentTile);
            CurrentBuilding = CurrentTile?.Building;
        }
    }



    private void UpdatePreview()
    {
        if (CurrentTile == null || !GameManager.IsInGame())
        {
            return;
        }

        InteractionPreview.transform.position = CurrentTile.transform.position;
        BuildPreviewSpriteRenderer.sortingOrder = TileManager.GetSortOrderFromPosition(CurrentTile.transform.position, 1);

        if (_currentInteractionIndex == 0)
        {
            // Earth
            _interactionText.text = "Build Earth";
            _interactionDescriptionText.text = "Crumbles without trees";
            if (RessourceManager.EnoughResources(_gameSettings.EarthPlacementCost, 0))
            {
                InteractionPreviewSpriteRenderer.color = previewBorderGreenColor;
                _earthCostText.color = greenColor;
            }
            else
            {
                InteractionPreviewSpriteRenderer.color = previewBorderRedColor;
                _earthCostText.color = redColor;
            }
            _waterCostText.color = greenColor;
            _earthCostText.text = _gameSettings.EarthPlacementCost.ToString();
            _waterCostText.text = "0";
        }
        else
        {
            if (Interactions[_currentInteractionIndex] is BuildingInteraction buildingInteraction)
            {
                BuildingPreset settings = buildingInteraction.Settings;
                _interactionDescriptionText.text = settings.Description;
                if (settings == RootManager.Lotus.BuildingPreset)
                {
                    if (RootManager.Lotus.currentUpgradeStage < settings.UpgradeStagesLength)
                    {
                        _interactionText.text = "Upgrade " + settings.DisplayName;
                        _interactionDescriptionText.text = CurrentTile != null && CurrentTile.Building == RootManager.Lotus ? settings.Description : "Go to Lotus to upgrade";
                        int index = RootManager.Lotus.currentUpgradeStage;
                        _earthCostText.text = settings.UpgradeStages[index].UpgradeEarthCosts.ToString();
                        _waterCostText.text = settings.UpgradeStages[index].UpgradeWaterCosts.ToString();
                    }
                    else
                    {
                        _interactionText.text = "Max upgrade reached";
                        _interactionDescriptionText.text = "Congratulations!";
                        _earthCostText.text = "0";
                        _waterCostText.text = "0";
                    }
                }
                else
                {
                    _interactionText.text = "Build " + settings.DisplayName;
                    _earthCostText.text = settings.EarthCost.ToString();
                    _waterCostText.text = settings.WaterCost.ToString();
                }
                _earthCostText.color = RessourceManager.Earth >= settings.EarthCost ? greenColor : redColor;
                _waterCostText.color = RessourceManager.Water >= settings.WaterCost ? greenColor : redColor;
                InteractionPreviewSpriteRenderer.color = buildingInteraction.CanBePlaced(CurrentTile) ? previewBorderGreenColor : previewBorderRedColor;
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed || !GameManager.IsInGame())
        {
            return;
        }

        if (CurrentTile == null)
        {
            return;
        }

        OnTileInteract?.Invoke(CurrentTile);
        _currentInteraction?.OnInteract(CurrentTile);
        CurrentTile = null;
        CurrentBuilding = null;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!GameManager.IsInGame())
        {
            return;
        }

        if (context.performed)
        {
            _moveInput = context.ReadValue<Vector2>();
            _interactionOffset = new Vector3(_moveInput.x, _moveInput.y, 0) * _interactionSettings.InteractionDistance;
        }
        else if(context.canceled)
        {
            _moveInput = Vector2.zero;
            _interactionOffset = Vector3.zero;
        }
    }

    public void OnNextBuilding(InputAction.CallbackContext context)
    {
        if (!context.performed || !GameManager.IsInGame())
        {
            return;
        }

        SelectInteraction((_currentInteractionIndex + 1) % Interactions.Length);
    }

    public void OnPreviousBuilding(InputAction.CallbackContext context)
    {
        if (!context.performed || !GameManager.IsInGame())
        {
            return;
        }

        if (_currentInteractionIndex == 0)
        {
            _currentInteractionIndex = Interactions.Length - 1;
        }
        else
        {
            _currentInteractionIndex -= 1;
        }

        SelectInteraction(_currentInteractionIndex);
    }

    public void OnBuildingKey(InputAction.CallbackContext context)
    {
        if (!context.performed || !GameManager.IsInGame())
        {
            return;
        }

        if (int.TryParse(context.control.name, out int index))
        {
            if (index == 0)
            {
                index = 10;
            }
        }

        SelectInteraction(index - 1);
    }

    private void OnDrawGizmos()
    {
        if (_interactionSettings != null && _interactionSettings.DrawGizmos)
        {
            Gizmos.color = redColor;
            Gizmos.DrawLine(transform.position, transform.position + _interactionOffset);
        }
    }

    private void OnDestroy()
    {
        InputManager.OnInteract -= OnInteract;
        InputManager.OnMove -= OnMove;
        InputManager.OnNextBuilding -= OnNextBuilding;
        InputManager.OnPrevBuilding -= OnPreviousBuilding;
        InputManager.OnBuildingKey -= OnBuildingKey;
    }
}