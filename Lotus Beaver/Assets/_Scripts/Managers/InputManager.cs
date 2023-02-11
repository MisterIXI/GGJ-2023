using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    public event Action<CallbackContext> OnMove;

    public event Action<CallbackContext> OnInteract;

    public event Action<CallbackContext> OnReset;

    public event Action<CallbackContext> OnPause;

    public event Action<CallbackContext> OnPrevBuilding;

    public event Action<CallbackContext> OnNextBuilding;

    public event Action<CallbackContext> OnBuildingKey;

    private PlayerInput _playerInput;

    private void Awake()
    {
        if (RefManager.inputManager != null)
        {
            Destroy(gameObject);
            return;
        }
        RefManager.inputManager = this;
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        SubscribeActions();
    }

    private void SubscribeActions()
    {
        InputActionMap map = _playerInput.actions.FindActionMap("Player");
        map["Move"].started += OnMoveInput;
        map["Move"].performed += OnMoveInput;
        map["Move"].canceled += OnMoveInput;

        map["Interact"].started += OnInteractInput;
        map["Interact"].performed += OnInteractInput;
        map["Interact"].canceled += OnInteractInput;

        map["Pause"].started += OnPauseInput;
        map["Pause"].performed += OnPauseInput;
        map["Pause"].canceled += OnPauseInput;

        map["PrevBuilding"].started += OnPrevBuildingInput;
        map["PrevBuilding"].performed += OnPrevBuildingInput;
        map["PrevBuilding"].canceled += OnPrevBuildingInput;

        map["NextBuilding"].started += OnNextBuildingInput;
        map["NextBuilding"].performed += OnNextBuildingInput;
        map["NextBuilding"].canceled += OnNextBuildingInput;

        map["BuildingKey"].started += OnBuildingKeyInput;
        map["BuildingKey"].performed += OnBuildingKeyInput;
        map["BuildingKey"].canceled += OnBuildingKeyInput;

        map["ResetPosition"].started += OnResetInput;
        map["ResetPosition"].performed += OnResetInput;
        map["ResetPosition"].canceled += OnResetInput;
    }

    public void OnMoveInput(CallbackContext context)
    {
        OnMove?.Invoke(context);
    }

    public void OnInteractInput(CallbackContext context)
    {
        OnInteract?.Invoke(context);
    }

    public void OnResetInput(CallbackContext context)
    {
        OnReset?.Invoke(context);
    }

    public void OnPauseInput(CallbackContext context)
    {
        OnPause?.Invoke(context);
    }

    public void OnPrevBuildingInput(CallbackContext context)
    {
        OnPrevBuilding?.Invoke(context);
    }

    public void OnNextBuildingInput(CallbackContext context)
    {
        OnNextBuilding?.Invoke(context);
    }

    public void OnBuildingKeyInput(CallbackContext context)
    {
        OnBuildingKey?.Invoke(context);
    }
}