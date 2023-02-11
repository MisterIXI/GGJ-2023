public class WaterInteraction : IInteractable
{
    private readonly int _index;
    private readonly InteractionController _interactionController;

    public WaterInteraction(InteractionController interactionController, int index)
    {
        InteractionController.OnInteractionChange += OnSelectionChange;
        _index = index;
        _interactionController = interactionController;
    }

    ~WaterInteraction()
    {
        InteractionController.OnInteractionChange -= OnSelectionChange;
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
    }

    public void OnSelection(Tile tile)
    {
    }
}