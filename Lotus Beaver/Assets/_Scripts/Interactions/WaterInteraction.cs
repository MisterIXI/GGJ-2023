public class WaterInteraction : Interactable
{
    private new readonly int _index;
    private new readonly InteractionController _interactionController;

    public WaterInteraction(InteractionPreset interactionPreset, InteractionController interactionController, int index) : base(interactionPreset, interactionController, index)
    {
    }

    ~WaterInteraction()
    {
        InteractionController.OnInteractionChange -= OnSelectionChange;
    }

    public override void OnSelectionChange(int index)
    {
        if (_index == index && _interactionController.BuildPreviewSpriteRenderer != null)
        {
            _interactionController.BuildPreviewSpriteRenderer.sprite = null;
        }
    }

    public override void OnInteract(Tile tile)
    {
    }

    public override void OnSelection(Tile tile)
    {
    }
}