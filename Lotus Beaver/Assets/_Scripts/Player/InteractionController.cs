using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private InteractionSettings _interactionSettings;
    [field: SerializeField] public InteractionPreview InteractionPreview { get; private set; }

    [SerializeField] private Color greenColor;
    [SerializeField] private Color redColor;
    [SerializeField] private Color previewBorderRedColor;
    [SerializeField] private Color previewBorderGreenColor;

    private IInteractable[] Interactions;

    private Vector2 _moveInput;
    private Vector3 _interactionOffset;
    private IInteractable _currentInteraction;
    private int _currentInteractionIndex;

    public SpriteRenderer InteractionPreviewSpriteRenderer => InteractionPreview.SpriteRenderer;
    [field: SerializeField] public SpriteRenderer BuildPreviewSpriteRenderer { get; private set; }
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
        if (tile == CurrentTile)
        {
            return;
        }

        CurrentTile = tile;
        OnTileSelectionChange?.Invoke(CurrentTile);
        _currentInteraction?.OnSelection(CurrentTile);
        CurrentBuilding = CurrentTile?.Building;
    }



    private void UpdatePreview()
    {
        if (CurrentTile == null || !GameManager.IsInGame())
        {
            return;
        }

        InteractionPreview.Transform.position = CurrentTile.transform.position;
        BuildPreviewSpriteRenderer.sortingOrder = TileManager.GetSortOrderFromPosition(CurrentTile.transform.position, 1);
        if (_currentInteractionIndex == 0)
        {
            // Earth
            HeadUpDisplayActiveElements.InteractionText.text = "Build Earth";
            HeadUpDisplayActiveElements.InteractionDescriptionText.text = "Crumbles without trees";
            if (RessourceManager.EnoughResources(GameSettingsManager.GameSettings().EarthPlacementCost, 0))
            {
                InteractionPreviewSpriteRenderer.color = previewBorderGreenColor;
                HeadUpDisplayActiveElements.CostEarthText.color = greenColor;
            }
            else
            {
                InteractionPreviewSpriteRenderer.color = previewBorderRedColor;
                HeadUpDisplayActiveElements.CostEarthText.color = redColor;
            }
            HeadUpDisplayActiveElements.CostEarthText.text = GameSettingsManager.GameSettings().EarthPlacementCost.ToString();
            HeadUpDisplayActiveElements.CostWaterText.color = greenColor;
            HeadUpDisplayActiveElements.CostWaterText.text = "0";
        }
        else
        {
            if (Interactions[_currentInteractionIndex] is BuildingInteraction buildingInteraction)
            {
                BuildingPreset settings = buildingInteraction.Settings;
                HeadUpDisplayActiveElements.InteractionDescriptionText.text = settings.Description;
                if (settings == RootManager.Lotus.BuildingPreset)
                {
                    if (RootManager.Lotus._currentUpgradeStage < settings.UpgradeStagesLength)
                    {
                        HeadUpDisplayActiveElements.InteractionText.text = "Upgrade " + settings.DisplayName;
                        HeadUpDisplayActiveElements.InteractionDescriptionText.text = CurrentTile != null && CurrentTile.Building == RootManager.Lotus ? settings.Description : "Go to Lotus to upgrade";
                        int index = RootManager.Lotus._currentUpgradeStage;
                        HeadUpDisplayActiveElements.CostEarthText.text = settings.UpgradeStages[index].UpgradeEarthCosts.ToString();
                        HeadUpDisplayActiveElements.CostWaterText.text = settings.UpgradeStages[index].UpgradeWaterCosts.ToString();
                    }
                    else
                    {
                        HeadUpDisplayActiveElements.InteractionText.text = "Max upgrade reached";
                        HeadUpDisplayActiveElements.InteractionDescriptionText.text = "Congratulations!";
                        HeadUpDisplayActiveElements.CostEarthText.text = "0";
                        HeadUpDisplayActiveElements.CostWaterText.text = "0";
                    }
                }
                else
                {
                    HeadUpDisplayActiveElements.InteractionText.text = "Build " + settings.DisplayName;
                    HeadUpDisplayActiveElements.CostEarthText.text = settings.EarthCost.ToString();
                    HeadUpDisplayActiveElements.CostWaterText.text = settings.WaterCost.ToString();
                }
                HeadUpDisplayActiveElements.CostEarthText.color = RessourceManager.Earth >= settings.EarthCost ? greenColor : redColor;
                HeadUpDisplayActiveElements.CostWaterText.color = RessourceManager.Water >= settings.WaterCost ? greenColor : redColor;
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
        else if (context.canceled)
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