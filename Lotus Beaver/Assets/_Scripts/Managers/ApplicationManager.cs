using System;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    private static ApplicationManager _instance;

    public static event Action OnQuit;

    public static bool IsQuitting { get; private set; }

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
        IsQuitting = true;

        OnQuit?.Invoke();
    }

    public void Quit()
    {
        OnApplicationQuit();
        Application.Quit();
    }
}