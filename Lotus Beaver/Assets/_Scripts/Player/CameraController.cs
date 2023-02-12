using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private MovementSettings _movementSettings;
    private float _lastDamping;
    private CinemachineVirtualCamera _vcam;
    private CinemachineHardLockToTarget _hardLockToTarget;

    private void Awake()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

        _vcam = GetComponent<CinemachineVirtualCamera>();
        _hardLockToTarget = _vcam.GetCinemachineComponent<CinemachineHardLockToTarget>();
        if (playerMovement != null)
        {
            _vcam.Follow = playerMovement.CameraTarget;
            _vcam.LookAt = playerMovement.CameraTarget;

        }
        _lastDamping = -1;
    }

    private void Update()
    {
        if (_lastDamping != _movementSettings.cameraDamping)
        {
            _hardLockToTarget.m_Damping = _movementSettings.cameraDamping;
            _lastDamping = _movementSettings.cameraDamping;
        }
    }
}