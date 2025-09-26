using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [Range(0f, 1f)]
    [SerializeField] private float _speedFactor;

    void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, _target.position, _speedFactor);
    }
}
