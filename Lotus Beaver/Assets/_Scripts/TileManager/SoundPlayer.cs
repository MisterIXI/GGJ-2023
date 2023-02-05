using System.Collections;
using UnityEngine;

public class SoundPlayer : Poolable
{
    [SerializeField] private AudioSource _audioSource;

    private SoundPool _pool;

    private Coroutine _playCoroutine;

    public void PlaySound()
    {
        _audioSource.PlayOneShot(_audioSource.clip);
    }

    public void PlaySound(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);

        if (_playCoroutine != null)
        {
            StopCoroutine(_playCoroutine);
        }

        _playCoroutine = StartCoroutine(AudioDurationCoroutine(audioClip.length));
    }

    private IEnumerator AudioDurationCoroutine(float audioClipLength)
    {
        yield return new WaitForSeconds(audioClipLength);
        SetActive(false);
    }

    public void SetPool(SoundPool pool)
    {
        _pool = pool;
    }

    public override void ReturnToPool()
    {
        _pool.ReturnToPool(this);
    }
}
