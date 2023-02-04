using UnityEngine;

public class HeadUpDisplayElement : MonoBehaviour
{
    public static HeadUpDisplayElement Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
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

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
}