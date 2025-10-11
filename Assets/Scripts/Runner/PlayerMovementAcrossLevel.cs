using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAcrossLevel : MonoBehaviour
{
    public DistanceTracker distanceTracker { get; private set; }
    public Vector3 CurrentDirection { get; set; }

    public static PlayerMovementAcrossLevel Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        CurrentDirection = Vector3.right; // изначально движение вправо
        distanceTracker = DistanceTracker.Instance;
    }

    void Update()
    {
        // Блокируем движение во время поворота через DistanceTracker

        //Debug.Log($"IsRunnerRotating: {distanceTracker.IsRunnerRotating}");
        //Debug.Log($"IsRunnerRotatingGlobal: {GlobalFlags.GetFlag(GlobalFlags.Flags.RUNNER_IS_ROTATING)}");
        if (distanceTracker.IsRunnerRotating) return;
        if (GlobalFlags.GetFlag(Flags.GameOver)) return;
        if (distanceTracker == null) return;
        transform.Translate(CurrentDirection * distanceTracker.CurrentSpeed * Time.deltaTime);
    }
}
