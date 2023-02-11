using System.Collections.Generic;
using UnityEngine;

public abstract class Pool<T> : MonoBehaviour, IPool<T> where T : Poolable
{
    [SerializeField] private T _poolable;
    [SerializeField] private int _initialSize;
    private readonly Queue<T> _poolables = new();

    public Transform Transform { get; private set; }

    protected virtual void Awake()
    {
        Transform = transform;
        InitializePool();
    }

    protected virtual void Start()
    {
    }

    private void InitializePool()
    {
        for (int i = 0; i < _initialSize; i++)
        {
            _ = GrowPool();
        }
    }

    private T GrowPool()
    {
        return CreatePoolable();
    }

    protected abstract void SetPool(T poolable);

    private T CreatePoolable()
    {
        T poolable = Instantiate(_poolable, Transform);
        poolable.PoolTransform = Transform;
        SetPool(poolable);
        poolable.SetActive(false);
        return poolable;
    }

    public T GetPoolable()
    {
        return _poolables.Count == 0 ? CreatePoolable() : _poolables.Dequeue();
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