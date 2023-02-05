using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSoundController : MonoBehaviour, ISelectHandler
{
    private void Awake()
    {
        GetComponent<Button>()?.onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        SoundManager.PlayMenuClick();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.PlayMenuHover();
    }

    private void OnDestroy()
    {
        GetComponent<Button>()?.onClick.RemoveListener(PlayClickSound);
    }
}
