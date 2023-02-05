using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundPool _walking;
    [SerializeField] private SoundPool _planting;
    [SerializeField] private SoundPool _menuClick;
    [SerializeField] private SoundPool _upgrade;
    [SerializeField] private SoundPool _waterSound;
    [SerializeField] private SoundPool _waterSplash;

    private static SoundManager _instance;

    private void Awake() {
        if(RefManager.soundManager != null)
        {
            Destroy(gameObject);
            return;
        }
        RefManager.soundManager = this;

        _instance = this;
    }

    public static void PlayWalking()
    {
        PlaySound(_instance._walking);
    }

    public static void PlayPlanting()
    {
        PlaySound(_instance._planting);
    }

    public static void PlayMenuClick()
    {
        PlaySound(_instance._menuClick);
    }

    public static void PlayUpgrade()
    {
        PlaySound(_instance._upgrade);
    }

    public static void PlayWaterSound()
    {
        PlaySound(_instance._waterSound);
    }

    public static void PlayWaterSplash()
    {
        PlaySound(_instance._waterSplash);
    }

    public static void PlaySound(SoundPool soundPool)
    {
        SoundPlayer soundPlayer = soundPool.GetPoolable();
        soundPlayer.PlaySound();
    }
}