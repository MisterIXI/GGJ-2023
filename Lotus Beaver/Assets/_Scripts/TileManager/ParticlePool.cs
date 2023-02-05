public class ParticlePool : Pool<ParticlePlayer>
{
    protected override void SetPool(ParticlePlayer poolable)
    {
        poolable.SetPool(this);
    }
}