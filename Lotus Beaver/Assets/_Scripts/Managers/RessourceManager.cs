using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    private static RessourceManager _instance;

    [SerializeField] private GameSettings _gameSettings;

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
    }

    public static void AddWater(int value)
    {
        water += value;
    }

    public static bool EnoughResources(int earthCost, int waterCost)
    {
        if(earthCost <= earth && waterCost <= water)
            return true;
        else
            return false;
    }

    public static void UseResources(int earthCost, int waterCost)
    {
        earth -= earthCost;
        water -= waterCost;
    }
}
