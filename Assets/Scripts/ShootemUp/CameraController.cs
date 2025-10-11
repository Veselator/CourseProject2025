using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private ICameraTracker _tracker;
    private Transform _target;
    public static bool IsAbleToUpdate = true;

    private void Start()
    {
        _target = PlayerMovementHandler.Instance.transform;

        _tracker = GetComponent<SoftCameraTracker>();
    }

    private void Update()
    {
        if (GlobalFlags.GetFlag(Flags.GameOver)) return;
        if (IsAbleToUpdate) transform.position = _tracker.GetCurrentPosition(_target.position);
    }
}
