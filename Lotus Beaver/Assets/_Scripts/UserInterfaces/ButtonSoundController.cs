using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSoundController : MonoBehaviour, ISelectHandler
{
    private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        SoundManager.PlayMenuClick();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.PlayMenuClick();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(PlayClickSound);
    }
}
