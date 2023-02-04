public interface IPool<T> where T : Poolable
{
    T GetPoolable();

    T Peek();

    void ReturnToPool(T poolable);
}