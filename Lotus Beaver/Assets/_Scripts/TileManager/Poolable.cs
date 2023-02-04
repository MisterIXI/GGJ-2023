using UnityEngine;

public abstract class Poolable : MonoBehaviour, IPoolable
{
    [SerializeField] protected Transform _transform;
    [SerializeField] protected GameObject _gameObject;

    public Transform Transform => _transform;
    public Transform PoolTransform { get; set; }

    public abstract void ReturnToPool();

    public void ParentToPoolalbe()
    {
        _transform.SetParent(PoolTransform);
    }

    protected virtual void OnDisable()
    {
        ReturnToPool();
    }

    public virtual void SetActive(bool active)
    {
        ParentToPoolalbe();
        _gameObject.SetActive(active);
    }
}