using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<GameState> OnGameStateChanged;

    public static GameState GameState { get; private set; }

    public static event Action OnNewGame;

    public static event Action OnGameOver;

    public static float GameStartTime { get; private set; }

    public static bool IsInGame() => GameState == GameState.Ingame;

    private void Awake()
    {
        if (RefManager.gameManager != null)
        {
            Destroy(gameObject);
            return;
        }
        RefManager.gameManager = this;
        DontDestroyOnLoad(transform.root.gameObject);

        SetGameState(GameState.GameOver);
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

        GameState = gameState;
        OnGameStateChanged?.Invoke(GameState);
    }

    public static void StartNewGame()
    {
#if UNITY_EDITOR
        Debug.Log("New Game");
#endif
        GameStartTime = Time.time;
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