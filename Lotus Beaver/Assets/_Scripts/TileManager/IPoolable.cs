using UnityEngine;

public interface IPoolable
{
    Transform Transform { get; }

    void ReturnToPool();

    void SetActive(bool active);
}