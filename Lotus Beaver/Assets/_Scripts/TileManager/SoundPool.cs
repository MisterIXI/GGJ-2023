public class SoundPool : Pool<SoundPlayer>
{
    protected override void SetPool(SoundPlayer poolable)
    {
        poolable.SetPool(this);
    }
}
