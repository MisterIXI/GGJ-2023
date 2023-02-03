using UnityEngine;

public class HeadUpDisplayElement : MonoBehaviour
{
    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Ingame:
                SetActive(true);
                break;
            case GameState.Paused:
            case GameState.GameOver:
                SetActive(false);
                break;
        }
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}