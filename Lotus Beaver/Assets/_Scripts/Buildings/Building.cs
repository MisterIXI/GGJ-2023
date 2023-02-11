using System;
using System.Linq;
using UnityEngine;

public class Building : InteractableBase
{
    [SerializeField] private BuildingPreset buildingPreset;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public BuildingPreset BuildingPreset => buildingPreset;
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public string BuildingName => buildingPreset.DisplayName;

    private int SelfHealAmount => buildingPreset.SelfHealAmount;
    private int HealAmount => buildingPreset.HealAmount;
    private int HealRadius => buildingPreset.HealRadius;

    private int TicksPerStage => buildingPreset.TicksPerStage;

    private int ConstructionStages => buildingPreset.ConstructionStagesLength;
    public int UpgradeStages => buildingPreset.UpgradeStagesLength;

    private bool isBuild;

    private int currentConstructionStage;
    private int constructionTicks;

    public int currentUpgradeStage;

    private Tile _tile;

    public void SetTile(Tile tile)
    {
        _tile = tile;
    }

    private void OnEnable()
    {
        TickManager.OnDamageTick += TickManager_OnDamageTick;

        currentConstructionStage = 0;
        constructionTicks = 0;
        currentUpgradeStage = 0;

        if (ConstructionStages > 0)
        {
            spriteRenderer.sprite = buildingPreset.ConstructionStages[0].Sprite;
            StartConstruction();
        }
        else
        {
            OnConstructionComplete();
        }

        StartConstruction();
    }

    private void StartConstruction()
    {
        TickManager.OnConstructionTick += TickManager_OnConstructionTick;
    }

    private void OnConstructionComplete()
    {
        if (ProducesEarthOrWater())
        {
            TickManager.OnBuildingTick += TickManager_OnBuildingTick;
        }
    }

    private bool ProducesEarthOrWater()
    {
        return buildingPreset.EarthProduction > 0 || buildingPreset.WaterProduction > 0;
    }

    private void TickManager_OnBuildingTick(object sender, TickManager.OnTickEventArgs e)
    {
        RessourceManager.AddEarth(buildingPreset.EarthProduction);
        RessourceManager.AddWater(buildingPreset.WaterProduction);
    }

    private void TickManager_OnConstructionTick(object sender, TickManager.OnTickEventArgs e)
    {
        if (!isBuild && currentConstructionStage < ConstructionStages)
        {
            constructionTicks++;
            if (constructionTicks >= TicksPerStage)
            {
                currentConstructionStage++;
                constructionTicks = 0;
                spriteRenderer.sprite = buildingPreset.ConstructionStages[currentConstructionStage].Sprite;

                if (currentConstructionStage == ConstructionStages)
                {
                    isBuild = true;

                    TickManager.OnConstructionTick -= TickManager_OnConstructionTick;

                    Animator anim = GetComponentInChildren<Animator>();
                    if (anim != null)
                    {
                        anim.enabled = true;
                        //anim.SetBool("isBuild", true);
                    }
                }
            }
        }
    }

    private void TickManager_OnDamageTick(object sender, TickManager.OnTickEventArgs e)
    {
        EarthController earthController = _tile?.TileElement?.TileController as EarthController;
        if (earthController != null)
        {
            earthController.GetHealth(SelfHealAmount);
            if (HealRadius > 0)
            {
                foreach (EarthController souroundingEarthController in TileManager.GetSurroundingTilesWithDiagonal(_tile).Select(x => x.TileElement?.TileController as EarthController))
                {
                    souroundingEarthController.GetHealth(HealAmount);
                }
            }
        }
    }

    public override void OnInteract(Tile tile)
    {
#if UNITY_EDITOR
        Debug.Log("Interacted with " + tile);
#endif
    }

    public override void OnSelection(Tile tile)
    {
#if UNITY_EDITOR
        Debug.Log("Selected " + tile);
#endif
    }

    public void Upgrade()
    {
        spriteRenderer.sprite = buildingPreset.UpgradeStages[currentUpgradeStage].Sprite;
        currentUpgradeStage++;
        if (buildingPreset.DisplayName == "Lotus")
        {
            int upgradeCount = GameSettingsManager.GameSettings().RootGrowthPerUpgrade;
            for (int i = 0; i < upgradeCount; i++)
            {
                RootManager.IncreaseRootLevel();
            }
        }
    }

    private void OnDisable()
    {
        _tile = null;

        TickManager.OnDamageTick -= TickManager_OnDamageTick;

        if (isBuild)
        {
            if (ProducesEarthOrWater())
            {
                TickManager.OnBuildingTick -= TickManager_OnBuildingTick;
            }
        }
        else
        {
            TickManager.OnConstructionTick -= TickManager_OnConstructionTick;
        }
    }
}