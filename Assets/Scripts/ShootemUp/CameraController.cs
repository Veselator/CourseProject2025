using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private ICameraTracker _tracker;
    private Transform _target;

    private void Start()
    {
        _target = PlayerMovementHandler.Instance.transform;

        _tracker = GetComponent<SoftCameraTracker>();
    }

    private void Update()
    {
        transform.position = _tracker.GetCurrentPosition(_target.position);
    }
}
