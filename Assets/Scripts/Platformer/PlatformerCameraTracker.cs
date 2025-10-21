using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCameraTracker : SoftCameraTracker
{
    [Header("Ќасколько позици€ мышки вли€ет на позицию камеры")]
    [SerializeField] private float _mouseImpactFactor = 0.2f;
    private Vector3 velocity = Vector3.zero;

    public override Vector3 GetCurrentPosition(Vector3 targetPosition)
    {
        return Vector3.SmoothDamp(transform.position, 
            Vector2.Lerp(targetPosition, GetMousePosition(), _mouseImpactFactor), 
            ref velocity, blendFactor);
    }
}
