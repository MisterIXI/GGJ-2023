using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    [SerializeField] private MovementSettings _movementSettings;
    private float _lastDamping;
    private CinemachineVirtualCamera _vcam;
    private CinemachineHardLockToTarget _hardLockToTarget;
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        _vcam = GetComponent<CinemachineVirtualCamera>();
        _hardLockToTarget = _vcam.GetCinemachineComponent<CinemachineHardLockToTarget>();
        if (player != null)
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
