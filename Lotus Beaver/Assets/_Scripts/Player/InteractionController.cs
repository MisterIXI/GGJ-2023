using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
public class InteractionController : MonoBehaviour
{
    [field: SerializeField] public GameObject InteractionPreview { get; private set; }

    private GameSettings _gameSettings;
    public SpriteRenderer InteractionPreviewSpriteRenderer { get; private set; }
    public SpriteRenderer BuildPreviewSpriteRenderer { get; private set; }
    private IInteractable[] Interactions;
    public static event Action<Tile> OnTileInteract;
    public static event Action<Tile> OnTileSelectionChange;
    public static event Action<int> OnInteractionChange;
    [SerializeField] private InteractionSettings _interactionSettings;
    public Tile CurrentTile { get; private set; }
    public InteractableBase CurrentBuilding { get; private set; }
    private SpriteRenderer _currentSpriteRenderer;
    private Color _originalColor;
    private TileManager _tileManager;
    private Vector2 _moveInput;
    private Vector3 _interactionOffset;
    private IInteractable _currentInteraction;
    private HeadUpDisplayActiveElements _hud;
    private int _currentInteractionIndex;
    private TextMeshProUGUI _interactionText;
    private TextMeshProUGUI _waterCostText;
    private TextMeshProUGUI _earthCostText;

    private void Start()
    {
        RefManager.inputManager.OnInteract += OnInteract;
        RefManager.inputManager.OnMove += OnMove;
        RefManager.inputManager.OnNextBuilding += OnNextBuilding;
        RefManager.inputManager.OnPrevBuilding += OnPreviousBuilding;
        RefManager.inputManager.OnBuildingKey += OnBuildingKey;
        InitializeInteractions();
        SelectInteraction(0);
        BuildPreviewSpriteRenderer = InteractionPreview.transform.GetChild(0).GetComponent<SpriteRenderer>();
        InteractionPreviewSpriteRenderer = InteractionPreview.GetComponent<SpriteRenderer>();
        _gameSettings = GameSettingsManager.GameSettings();
        _hud = HeadUpDisplayActiveElements.Instance;
        _interactionText = _hud.InteractionText;
        _waterCostText = _hud.CostWaterText;
        _earthCostText = _hud.CostEarthText;
    }
    private void Update()
    {
        TileSelectionUpdate();
        UpdatePreview();

    }

    private void InitializeInteractions()
    {
        Interactions = new IInteractable[2 + _interactionSettings.BuildingSettings.Length];
        Interactions[0] = new EarthInteraction(this, 0);
        Interactions[1] = new WaterInteraction(this, 1);
        for (int i = 0; i < _interactionSettings.BuildingSettings.Length; i++)
        {
            Interactions[i + 2] = new BuildingInteraction(_interactionSettings.BuildingSettings[i], this, i + 2);
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
            CurrentBuilding = CurrentTile?.building;
        }
    }

    [field: SerializeField] private Color greenColor = new Color(111 / 255, 157 / 255, 129 / 255, 1f);
    [field: SerializeField] private Color redColor = new Color(229 / 255, 133 / 255, 140 / 255, 1f);
    private void UpdatePreview()
    {
        // TODO: Refactor this
        if (CurrentTile != null && Time.timeScale != 0)
        {
            InteractionPreview.transform.position = CurrentTile.transform.position;
            if (_currentInteractionIndex == 0)
            {
                // Earth
                _interactionText.text = "Build Earth";
                if (RessourceManager.EnoughResources(_gameSettings.EarthPlacementCost, 0))
                {
                    InteractionPreviewSpriteRenderer.color = greenColor;
                    _earthCostText.color = greenColor;
                }
                else
                {
                    InteractionPreviewSpriteRenderer.color = redColor;
                    _earthCostText.color = redColor;
                }
                _earthCostText.text = _gameSettings.EarthPlacementCost.ToString();
                _waterCostText.text = "0";
            }
            else if (_currentInteractionIndex == 1)
            {
                // water
                InteractionPreviewSpriteRenderer.color = redColor;
                _interactionText.text = "No use yet";
                _earthCostText.text = "0";
                _waterCostText.text = "0";
            }
            else
            {
                if (Interactions[_currentInteractionIndex] is BuildingInteraction buildingInteraction)
                {
                    var settings = buildingInteraction.Settings;
                    if (CurrentTile.building != null && CurrentTile.building?.buildingName == "Lotus" && settings.displayName == "Lotus")
                    {
                        _interactionText.text = "Upgrade " + settings.displayName;

                        int index = CurrentTile.building.currentUpgradeStage;
                        _earthCostText.text = CurrentTile.building.upgradeEarthCosts[index].ToString();
                        _waterCostText.text = CurrentTile.building.upgradeWaterCosts[index].ToString();
                    }
                    else
                    {
                        if (CurrentTile.building?.buildingName == "Lotus" && settings.displayName == "Lotus")
                            _interactionText.text = "No Lotus to upgrade";
                        else
                            _interactionText.text = "Build " + settings.displayName;
                        _earthCostText.text = settings.earthCost.ToString();
                        _waterCostText.text = settings.waterCost.ToString();

                    }
                    if (RessourceManager.earth >= settings.earthCost)
                        _earthCostText.color = greenColor;
                    else
                        _earthCostText.color = redColor;
                    if (RessourceManager.water >= settings.waterCost)
                        _waterCostText.color = greenColor;
                    else
                        _waterCostText.color = redColor;
                    if (buildingInteraction.CanBePlaced(CurrentTile))
                    {
                        InteractionPreviewSpriteRenderer.color = greenColor;
                    }
                    else
                    {
                        InteractionPreviewSpriteRenderer.color = redColor;
                    }
                }
            }

        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeScale != 0)
        {
            if (CurrentTile != null)
            {
                // if(CurrentBuilding != null)
                //     CurrentBuilding.OnInteract(CurrentTile);
                OnTileInteract?.Invoke(CurrentTile);
                _currentInteraction?.OnInteract(CurrentTile);
                CurrentTile = null;
                CurrentBuilding = null;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeScale != 0)
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
        if (context.performed && Time.timeScale != 0)
        {
            SelectInteraction((_currentInteractionIndex + 1) % Interactions.Length);
        }
    }

    public void OnPreviousBuilding(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeScale != 0)
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
        if (context.performed && Time.timeScale != 0)
        {
            // try parse the name of the control to an int
            int index = 1;
            int.TryParse(context.control.name, out index);
            if (index == 0)
                index = 10;
            SelectInteraction(index - 1);
        }
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
        RefManager.inputManager.OnInteract -= OnInteract;
        RefManager.inputManager.OnMove -= OnMove;
        RefManager.inputManager.OnNextBuilding -= OnNextBuilding;
        RefManager.inputManager.OnPrevBuilding -= OnPreviousBuilding;
        RefManager.inputManager.OnBuildingKey -= OnBuildingKey;
    }
}