using UnityEngine;

[CreateAssetMenu(fileName = "InteractionSettings", menuName = "Lotus Beaver/InteractionSettings", order = 0)]
public class InteractionSettings : ScriptableObject {
    public bool DrawGizmos = false;
    [Range(0f,10f)]public float InteractionDistance = 1.5f;
}