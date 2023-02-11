using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] protected TileElement _tileElement;

    protected Tile _parentTile;
    public Tile ParentTile => _parentTile ??= GetComponentInParent<Tile>(true);

    protected virtual void OnDisable()
    {
        _parentTile = null;
    }
}
