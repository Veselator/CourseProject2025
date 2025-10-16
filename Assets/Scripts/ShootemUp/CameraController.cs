using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private ICameraTracker _tracker;
    private Transform _target;
    public static bool IsAbleToUpdate = true;
    [SerializeField] private Vector3 _defaultTrackingPosition = Vector3.zero;

    private void Start()
    {
        if (PlayerMovementHandler.Instance != null) _target = PlayerMovementHandler.Instance.transform;

        _tracker = GetComponent<SoftCameraTracker>();
    }

    private void Update()
    {
        if (GlobalFlags.GetFlag(Flags.GameOver)) return;
        if (!IsAbleToUpdate) return; 
        if(_target != null) transform.position = _tracker.GetCurrentPosition(_target.position);
        else transform.position = _tracker.GetCurrentPosition(_defaultTrackingPosition);
    }
}
