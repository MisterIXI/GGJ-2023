using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;

    private static GameSettingsManager _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(transform.root.gameObject);
    }

    public static GameSettings GameSettings()
    {
        return _instance._gameSettings;
    }
}