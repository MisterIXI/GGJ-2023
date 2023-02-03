using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "Lotus Beaver/MovementSettings", order = 0)]
public class MovementSettings : ScriptableObject {
    [Header("Movement")]
    [Range(0.01f,10f)] public float moveSpeed = 1f;
    [Range(0.01f,1f)] public float acceleration = 0.7f;

    [Header("Camera")]
    [Range(0f,10f)] public float cameraDistance = 1f;
    [Range(0f,10f)] public float cameraDamping = 1f;
    [Range(5f,100f)] public float cameraZDistance = 10f;
}