using System;
using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    private static RessourceManager _instance;

    [SerializeField] private GameSettings _gameSettings;

    public static event Action<float> OnEarthChange;

    public static event Action<float> OnWaterChange;

    public static float Earth;
    public static float Water;

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

    private void Start()
    {
#if UNITY_EDITOR
        Debug.Log(_gameSettings.startWater);
#endif
        Earth = _gameSettings.startEarth;
        Water = _gameSettings.startWater;
        OnEarthChange?.Invoke(Earth);
        OnWaterChange?.Invoke(Water);
    }

    public static void AddEarth(float value)
    {
        Earth += value;
        OnEarthChange?.Invoke(Earth);
    }

    public static void AddWater(float value)
    {
        Water += value;
        OnWaterChange?.Invoke(Water);
    }

    public static bool EnoughResources(float earthCost, float waterCost)
    {
        return earthCost <= Earth && waterCost <= Water;
    }

    public static void UseResources(float earthCost, float waterCost)
    {
        Earth -= earthCost;
        Water -= waterCost;
        OnEarthChange?.Invoke(Earth);
        OnWaterChange?.Invoke(Water);
    }
}