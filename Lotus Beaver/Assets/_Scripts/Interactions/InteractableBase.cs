using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    public virtual void OnInteract(Tile tile)
    {
#if UNITY_EDITOR
        Debug.Log("Interacted with " + tile);
#endif
    }

    public virtual void OnSelection(Tile tile)
    {
#if UNITY_EDITOR
        Debug.Log("Selected " + tile);
#endif
    }
}