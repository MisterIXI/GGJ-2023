using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<GameState> OnGameStateChanged;
    private static GameState _gameState;
    public static GameState GameState => _gameState;

    public static event Action OnNewGame;
    public static event Action OnGameOver;

    private void Awake()
    {
        if (RefManager.gameManager != null)
        {
            Destroy(gameObject);
            return;
        }
        RefManager.gameManager = this;
        SceneManager.activeSceneChanged += OnSceneChanged;
        DontDestroyOnLoad(transform.root.gameObject);

        SetGameState(GameState.GameOver);
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        if (next.name == "Game")
        {
            // Do something
        }
    }

    public static void SetGameState(GameState gameState)
    {
#if UNITY_EDITOR
        Debug.Log($"Game State: {gameState}");
#endif

        switch (gameState)
        {
            case GameState.Ingame:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
        }

        _gameState = gameState;
        OnGameStateChanged?.Invoke(_gameState);
    }

    public static void StartNewGame()
    {
#if UNITY_EDITOR
        Debug.Log("New Game");
#endif

        OnNewGame?.Invoke();
    }

    [ContextMenu(nameof(LoseGame))]
    public void LoseGame()
    {
#if UNITY_EDITOR
        Debug.Log("Lose Game");
#endif

        SetGameState(GameState.GameOver);

        OnGameOver?.Invoke();
    }
}
