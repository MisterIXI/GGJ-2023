public class Interactable : IInteractable
{
    protected readonly int _index;
    protected readonly InteractionController _interactionController;
    public InteractionPreset InteractionPreset { get; set; }

    public Interactable(InteractionPreset interactionPreset, InteractionController interactionController, int index)
    {
        InteractionPreset = interactionPreset;
        InteractionController.OnInteractionChange += OnSelectionChange;
        _index = index;
        _interactionController = interactionController;
    }

    public virtual void OnInteract(Tile tile)
    {
        if (tile.TileElement?.TileElementType != TileElementType.Root && RessourceManager.Earth >= GameSettingsManager.GameSettings().EarthPlacementCost)
        {
            RessourceManager.Earth -= GameSettingsManager.GameSettings().EarthPlacementCost;
            TileManager.SetTileElementType(tile, TileElementType.Earth, out _);
            SoundManager.PlayPlanting();
        }
        else
        {
            SoundManager.PlayError();
        }
    }

    public virtual void OnSelection(Tile tile)
    {
    }

    public virtual void OnSelectionChange(int selectedIndex)
    {
        if (selectedIndex == _index)
        {
            _interactionController.BuildPreviewSpriteRenderer.sprite = null;
        }
    }
}