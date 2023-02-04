using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RessourceManager : MonoBehaviour
{
    private static RessourceManager _instance;

    [SerializeField] private GameSettings _gameSettings;

    public static event Action<int> OnEarthChange;
    public static event Action<int> OnWaterChange;

    public static int earth;
    public static int water;

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

    public static void AddEarth(int value)
    {
        earth += value;
        OnEarthChange?.Invoke(earth);
    }

    public static void AddWater(int value)
    {
        water += value;
        OnWaterChange?.Invoke(water);
    }
}