public class EarthInteraction : Interactable
{
    public EarthInteraction(InteractionPreset interactionPreset, InteractionController interactionController, int index) : base(interactionPreset, interactionController, index)
    {
    }

    public override void OnInteract(Tile tile)
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
}