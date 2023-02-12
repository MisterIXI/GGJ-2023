using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TileElement))]
public class EarthController : TileController
{
    [SerializeField] private float _health;
    [SerializeField] private DamageStageLibrary _damageStageLibrary;
    [SerializeField] private DamageStage _currentDamageStage;

    private void OnEnable()
    {
        _health = _damageStageLibrary.EarhMaxHealth;

        _currentDamageStage = _damageStageLibrary.DamageStages[0];
        if (Time.time - GameManager.GameStartTime > 0.5f)
        {
            _ = (GameManager.Instance.StartCoroutine(DelayRoutine()));
        }

        UpdateDamageSprite();
    }

    private IEnumerator DelayRoutine()
    {
        yield return null;

        TileCreationPool.ParticlePool?.GetPoolable()?.Play(_tileElement.Transform.position);
    }

    private void UpdateDamageStage()
    {
        DamageStage damageStage = _damageStageLibrary.GetCurrentDamageStage(_health);

        if (_currentDamageStage == damageStage)
        {
            return;
        }

        _currentDamageStage = damageStage;

        UpdateDamageSprite();
    }

    private void UpdateDamageSprite()
    {
        _tileElement.SpriteRenderer.sprite = _currentDamageStage.Sprite;
    }

    public void LoseHealth(float healthPoints)
    {
        _health -= healthPoints;

        if (_health <= 0f)
        {
            TakeFatalDamage();
            return;
        }

        UpdateDamageStage();
    }

    public void GetHealth(float healthPoints)
    {
        _health = Mathf.Min(_damageStageLibrary.EarhMaxHealth, _health + healthPoints);

        UpdateDamageStage();
    }

    private void TakeFatalDamage()
    {
        if (TileManager.IsSurroundedByWaterOrCliff(ParentTile))
        {
            TileManager.SetTileElementType(ParentTile, TileElementType.Water, out _);
        }
        else
        {
            TileManager.SetTileElementType(ParentTile, TileElementType.Cliff, out _);
        }
        SoundManager.PlayWaterSplash();
        if (Time.time - GameManager.GameStartTime > 0.5f)
        {
            TileDestroyPool.ParticlePool?.GetPoolable()?.Play(_tileElement.Transform.position);
        }
    }


#if UNITY_EDITOR
    [ContextMenu(nameof(TestTakeDamage))]
    private void TestTakeDamage()
    {
        LoseHealth(25f);
    }

    [ContextMenu(nameof(GetHealth))]
    private void GetHealth()
    {
        GetHealth(25f);
    }
#endif
}