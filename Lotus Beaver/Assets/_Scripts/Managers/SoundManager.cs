using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private AudioMixerSnapshot _inGame;
    [SerializeField] private AudioMixerSnapshot _paused;
    [SerializeField] private float _transitionTime;

    [SerializeField] private SoundPool _walking;
    [SerializeField] private SoundPool _planting;
    [SerializeField] private SoundPool _menuClick;
    [SerializeField] private SoundPool _upgrade;
    [SerializeField] private SoundPool _waterSound;
    [SerializeField] private SoundPool _waterSplash;
    [SerializeField] private SoundPool _error;
    [SerializeField] private SoundPool _menuHover;
    [SerializeField] private SoundPool _selectInteraction;

    private static SoundManager _instance;

    private const string _sFXSettings = "SFXSettings";
    private const string _musicSettings = "MusicSettings";

    private void Awake() {
        if(RefManager.soundManager != null)
        {
            Destroy(gameObject);
            return;
        }
        RefManager.soundManager = this;

        _instance = this;

        GameManager.OnGameStateChanged += OnGameStateChanged;

        Button[] buttons = FindObjectsOfType<Button>(true);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].AddComponent<ButtonSoundController>();
        }
    }

    private void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Ingame:
                _inGame.TransitionTo(_transitionTime);
                break;
            case GameState.Paused:
            case GameState.GameOver:
                _paused.TransitionTo(_transitionTime);
                break;
        }
    }

    public float LogSoundVolume(float volume)
    {
        if (volume <= 0.05f)
        {
            return -80f;
        }

        return Mathf.Log10(volume) * 20f;
    }

    [ContextMenu(nameof(StopMusic))]
    public void StopMusic()
    {
        _audioMixer.SetFloat(_musicSettings, value: -80f);
    }

    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat(_musicSettings, LogSoundVolume(volume));
    }

    public void SetSoundVolume(float volume)
    {
        _audioMixer.SetFloat(_sFXSettings, LogSoundVolume(volume));
    }

    public static void PlayWalking(Vector3? position = null)
    {
        PlaySound(_instance._walking, position);
    }

    public static void PlayMenuHover(Vector3? position = null)
    {
        // PlaySound(_instance._menuHover, position);
    }

    public static void PlaySelectInteraction(Vector3? position = null)
    {
        PlaySound(_instance._menuHover, position);
    }

    public static void PlayError(Vector3? position = null)
    {
        PlaySound(_instance._error, position);
    }

    public static void PlayPlanting(Vector3? position = null)
    {
        PlaySound(_instance._planting, position);
    }

    public static void PlayMenuClick(Vector3? position = null)
    {
        PlaySound(_instance._menuClick, position);
    }

    public static void PlayUpgrade(Vector3? position = null)
    {
        PlaySound(_instance._upgrade, position);
    }

    public static void PlayWaterSound(Vector3? position = null)
    {
        PlaySound(_instance._waterSound, position);
    }

    public static void PlayWaterSplash(Vector3? position = null)
    {
        PlaySound(_instance._waterSplash, position);
    }

    public static void PlaySound(SoundPool soundPool, Vector3? position = null)
    {
        SoundPlayer soundPlayer = soundPool.GetPoolable();
        soundPlayer.Transform.SetParent(null);
        soundPlayer.Transform.position = position.HasValue? position.Value : Vector3.zero;
        soundPlayer.PlaySound();
    }
}