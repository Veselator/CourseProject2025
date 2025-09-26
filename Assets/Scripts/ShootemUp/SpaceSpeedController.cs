using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSpeedController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float topY;
    [SerializeField] private float bottomY;
    public float Speed => speed;
    public float TopY => topY;
    public float BottomY => bottomY;

    public static SpaceSpeedController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
}
