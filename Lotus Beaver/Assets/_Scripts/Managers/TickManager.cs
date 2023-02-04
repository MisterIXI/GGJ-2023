using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TickManager : MonoBehaviour
{
    private static TickManager _instance;

    [SerializeField] private GameSettings _gameSettings;

    public class OnTickEventArgs : EventArgs
    {
        public int tick;
    }

    public static event EventHandler<OnTickEventArgs> OnDamageTick;
    public static event EventHandler<OnTickEventArgs> OnBuildingTick;

    private float damageTickTimerMax;
    private float buildingTickTimerMax;

    private int damageTick;
    private int buildingTick;

    private float damageTickTimer;
    private float buildingTickTimer;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        damageTick = 0;
        buildingTick = 0;
        damageTickTimerMax = _gameSettings.DamageTickTime;
        buildingTickTimerMax = _gameSettings.BuildingTickTime;
    }

    private void Update()
    {
        damageTickTimer += Time.deltaTime;
        buildingTickTimer += Time.deltaTime;

        if (damageTickTimer >= damageTickTimerMax)
        {
            damageTick++;
            damageTickTimer -= damageTickTimerMax;
            if (OnDamageTick != null) OnDamageTick(this, new OnTickEventArgs { tick = damageTick});
        }

        if(buildingTickTimer >= buildingTickTimerMax)
        {
            buildingTick++;
            buildingTickTimer -= buildingTickTimerMax;
            if (OnBuildingTick != null) OnBuildingTick(this, new OnTickEventArgs { tick = buildingTick });
        }
    }

}
