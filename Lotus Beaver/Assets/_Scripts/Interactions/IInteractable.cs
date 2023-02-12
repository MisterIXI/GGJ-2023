public interface IInteractable
{
    public void OnInteract(Tile tile);

    public void OnSelection(Tile tile);

    public InteractionPreset InteractionPreset { get; set; }
}