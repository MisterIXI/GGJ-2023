using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RessourceManager : MonoBehaviour
{
    private static RessourceManager _instance;

    [SerializeField] private GameSettings _gameSettings;

    public static event Action<float> OnEarthChange;
    public static event Action<float> OnWaterChange;

    public static float earth;
    public static float water;

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
        earth += _gameSettings.startEarth;
        water += _gameSettings.startWater;
        OnEarthChange?.Invoke(earth);
        OnWaterChange?.Invoke(water);
    }

    public static void AddEarth(float value)
    {
        earth += value;
        OnEarthChange?.Invoke(earth);
    }

    public static void AddWater(float value)
    {
        water += value;
        OnWaterChange?.Invoke(water);
    }

    public static bool EnoughResources(float earthCost, float waterCost)
    {
        if (earthCost <= earth && waterCost <= water)
            return true;
        else
            return false;
    }

    public static void UseResources(float earthCost, float waterCost)
    {
        earth -= earthCost;
        water -= waterCost;
        OnEarthChange?.Invoke(earth);
        OnWaterChange?.Invoke(water);
    }
}
