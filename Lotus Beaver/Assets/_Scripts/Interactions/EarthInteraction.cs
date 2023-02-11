public class EarthInteraction : IInteractable
{
    private readonly int _index;
    private readonly InteractionController _interactionController;

    public EarthInteraction(InteractionController interactionController, int index)
    {
        InteractionController.OnInteractionChange += OnSelectionChange;
        _index = index;
        _interactionController = interactionController;
    }

    public void OnSelectionChange(int index)
    {
        if (_index == index && _interactionController.BuildPreviewSpriteRenderer != null)
        {
            _interactionController.BuildPreviewSpriteRenderer.sprite = null;
        }
    }

    public void OnInteract(Tile tile)
    {
        if (tile.TileElement?.TileElementType != TileElementType.Root && RessourceManager.Earth >= GameSettingsManager.GameSettings().EarthPlacementCost)
        {
            RessourceManager.Earth -= GameSettingsManager.GameSettings().EarthPlacementCost;
            TileManager.SetTileElementType(tile, TileElementType.Earth);
            SoundManager.PlayPlanting();
        }
        else
        {
            SoundManager.PlayError();
        }
    }

    public void OnSelection(Tile tile)
    {
    }
}