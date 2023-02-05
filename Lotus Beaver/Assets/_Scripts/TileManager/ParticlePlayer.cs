using System.Collections;
using UnityEngine;

public class ParticlePlayer : Poolable
{
    [SerializeField] private ParticleSystem _particleSystem;

    private ParticlePool _pool;

    private Coroutine _playCoroutine;

    public void Play(Vector3? position)
    {
        _transform.SetParent(null);

        if (position.HasValue)
        {
            _transform.position = position.Value;
        }

        SetActive(true);

        _particleSystem.Play();

        if (_playCoroutine != null)
        {
            StopCoroutine(_playCoroutine);
        }

        _playCoroutine = StartCoroutine(ParticleCoroutine(_particleSystem.main.duration * 2f));
    }

    private IEnumerator ParticleCoroutine(float audioClipLength)
    {
        yield return new WaitForSeconds(audioClipLength);
        SetActive(false);
    }

    public void SetPool(ParticlePool pool)
    {
        _pool = pool;
    }

    public override void ReturnToPool()
    {
        _pool.ReturnToPool(this);
    }

    public override void SetActive(bool active)
    {
        _gameObject.SetActive(active);
    }
}
