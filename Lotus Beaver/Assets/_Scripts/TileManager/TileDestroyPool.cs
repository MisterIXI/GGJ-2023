﻿using UnityEngine;

public class TileDestroyPool : MonoBehaviour
{
    [SerializeField] private ParticlePool _particlePool;

    private static TileDestroyPool _instance;

    public static ParticlePool ParticlePool
    {
        get
        {
            return _instance?._particlePool;
        }
    }

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
}
