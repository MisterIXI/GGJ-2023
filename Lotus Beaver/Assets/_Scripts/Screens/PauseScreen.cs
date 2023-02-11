using UnityEngine.InputSystem;

public class PauseScreen : Screen
{
    private void Start()
    {
        RefManager.inputManager.OnPause += OnPauseButton;
        gameObject.SetActive(false);
    }

    public void OnPauseButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetActive(!gameObject.activeSelf);
        }
    }

    public void ResumeGame()
    {
        if (GameManager.GameState == GameState.GameOver)
        {
            GameManager.StartNewGame();
        }

        SetActive(false);
    }

    public override void SetActive(bool active)
    {
        base.SetActive(active);

        GameManager.SetGameState(active ? GameState.Paused : GameState.Ingame);
    }

    public void BackToMenu()
    {
        base.SetActive(false);
    }

    private void OnDestroy()
    {
        RefManager.inputManager.OnPause -= OnPauseButton;
    }
}