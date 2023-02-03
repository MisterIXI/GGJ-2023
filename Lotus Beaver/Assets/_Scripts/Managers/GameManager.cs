using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public enum GameState { Game, Paused, GameOver }
    public event Action<GameState> OnGameStateChanged;
    private GameState _gameState;
    private void Awake()
    {
        if (RefManager.gameManager != null)
        {
            Destroy(gameObject);
            return;
        }
        RefManager.gameManager = this;
        SceneManager.activeSceneChanged += OnSceneChanged;
        DontDestroyOnLoad(gameObject);
    }


    private void OnSceneChanged(Scene current, Scene next)
    {
        if (next.name == "Game")
        {
            // Do something
        }
    }

    public void SetGameState(GameState gameState)
    {
        _gameState = gameState;
        OnGameStateChanged?.Invoke(_gameState);
    }

}