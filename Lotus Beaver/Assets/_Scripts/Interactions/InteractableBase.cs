using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable {
    public virtual void OnInteract(Tile tile) {
        Debug.Log("Interacted with " + tile);
    }

    public virtual void OnSelection(Tile tile) {
        Debug.Log("Selected " + tile);
    }
}