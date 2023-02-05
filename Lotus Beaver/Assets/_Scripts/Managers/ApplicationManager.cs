using System;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    private static ApplicationManager _instance;

    private static bool _isQuitting;

    public static event Action OnQuit;

    public static bool IsQuitting => _isQuitting;

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

    private void OnApplicationQuit()
    {
        _isQuitting = true;

        OnQuit?.Invoke();
    }
}