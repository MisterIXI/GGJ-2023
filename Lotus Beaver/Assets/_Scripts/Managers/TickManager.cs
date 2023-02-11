using System;
using UnityEngine;

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

    public static event EventHandler<OnTickEventArgs> OnConstructionTick;

    private float DamageTickTimerMax => _gameSettings.DamageTickTime;
    private float BuildingTickTimerMax => _gameSettings.BuildingTickTime;
    private float ConstructionTickTimerMax=> _gameSettings.ConstructionTickTime;

    private int damageTick;
    private int buildingTick;
    private int constructionTick;

    private float damageTickTimer;
    private float buildingTickTimer;
    private float constructionTickTimer;

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

    private void OnEnable()
    {
        damageTick = 0;
        buildingTick = 0;
        constructionTick = 0;
    }

    private void Update()
    {
        damageTickTimer += Time.deltaTime;
        buildingTickTimer += Time.deltaTime;
        constructionTickTimer += Time.deltaTime;

        if (damageTickTimer >= DamageTickTimerMax)
        {
            damageTick++;
            damageTickTimer -= DamageTickTimerMax;
            OnDamageTick?.Invoke(this, new OnTickEventArgs { tick = damageTick });
        }

        if (buildingTickTimer >= BuildingTickTimerMax)
        {
            buildingTick++;
            buildingTickTimer -= BuildingTickTimerMax;
            OnBuildingTick?.Invoke(this, new OnTickEventArgs { tick = buildingTick });
        }

        if (constructionTickTimer >= ConstructionTickTimerMax)
        {
            constructionTick++;
            constructionTickTimer -= ConstructionTickTimerMax;
            OnConstructionTick?.Invoke(this, new OnTickEventArgs { tick = constructionTick });
        }
    }
}