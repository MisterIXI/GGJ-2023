using UnityEngine;

public abstract class Poolable : MonoBehaviour, IPoolable
{
    [SerializeField] protected Transform _transform;
    [SerializeField] protected GameObject _gameObject;

    public Transform Transform => _transform;

    public abstract void ReturnToPool();

    protected virtual void OnDisable()
    {
        ReturnToPool();
    }

    public virtual void SetActive(bool active)
    {
        _gameObject.SetActive(active);
    }
}