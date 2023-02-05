using UnityEngine;

public class TileCreationPool : MonoBehaviour
{
    [SerializeField] private ParticlePool _particlePool;

    private static TileCreationPool _instance;

    public static ParticlePool ParticlePool => _instance._particlePool;

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