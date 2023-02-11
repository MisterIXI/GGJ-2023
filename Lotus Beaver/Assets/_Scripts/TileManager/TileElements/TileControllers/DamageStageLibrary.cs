using UnityEngine;

[CreateAssetMenu(fileName = "DamageStageLibrary", menuName = "ScriptableObjects/DamageStageLibrary", order = 1)]
public class DamageStageLibrary : ScriptableObject
{
    [SerializeField] private DamageStage[] _damageStages;
    [SerializeField] private float _earthMaxHealth;
    [SerializeField] private float _baseDamage;

    public float BaseDamage => _baseDamage;

    public DamageStage[] DamageStages => _damageStages;
    public float EarhMaxHealth => _earthMaxHealth;

    public DamageStage GetCurrentDamageStage(float health)
    {
        for (int i = 0; i < _damageStages.Length; i++)
        {
            if (health >= _damageStages[i].HealthTreshHold)
            {
                return _damageStages[i];
            }
        }

        return _damageStages[^1];
    }
}