using System.Collections.Generic;
using UnityEngine;

public abstract class Pool<T> : MonoBehaviour, IPool<T> where T : Poolable
{
    [SerializeField] private T _poolable;
    [SerializeField] private int _initialSize;
    private Queue<T> _poolables = new Queue<T>();

    private Transform _transform;

    public Transform Transform  => _transform;

    protected virtual void Awake()
    {
        _transform = transform;
        InitializePool();
    }

    protected virtual void Start()
    {
    }

    private void InitializePool()
    {
        for (int i = 0; i < _initialSize; i++)
        {
            GrowPool();
        }
    }

    private T GrowPool()
    {
        return CreatePoolable();
    }

    protected abstract void SetPool(T poolable);

    private T CreatePoolable()
    {
        T poolable = Instantiate(_poolable, _transform);
        poolable.PoolTransform = _transform;
        SetPool(poolable);
        poolable.SetActive(false);
        return poolable;
    }

    public T GetPoolable()
    {
        if (_poolables.Count == 0)
        {
            return CreatePoolable();
        }
        else
        {
            return _poolables.Dequeue();
        }
    }

    public T Peek()
    {
        return _poolables.Peek();
    }

    public void ReturnToPool(T poolable)
    {
        if (_poolables.Contains(poolable))
        {
            return;
        }

        _poolables.Enqueue(poolable);
    }
}