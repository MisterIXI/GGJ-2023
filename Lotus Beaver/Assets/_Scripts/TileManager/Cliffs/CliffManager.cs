using UnityEngine;

public class CliffManager : MonoBehaviour
{
    [SerializeField] private CliffSpriteLibrary _cliffSpriteLibrary;

    private static CliffManager _instance;

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

    public static CliffSpriteLibrary CliffSpriteLibrary()
    {
        if (_instance == null)
        {
            return null;
        }

        return _instance._cliffSpriteLibrary;
    }
}
