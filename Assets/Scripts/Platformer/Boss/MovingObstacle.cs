using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MovingPlatform
{
    // Используется для босса
    private PlatformerObstaclesSpawner _linkedSpawner;
    public void Init(PlatformerObstaclesSpawner spawner, float newSpeed, Transform fromPoint, Transform toPoint)
    {
        _linkedSpawner = spawner;
        speed = newSpeed;
        points = new Transform[2] { fromPoint, toPoint };
        _isLooped = false;
    }

    private void Start()
    {
        OnMovementEnded += EndMovement;
    }

    private void EndMovement()
    {
        OnMovementEnded -= EndMovement;
        _linkedSpawner.HandleObstacleDestroyed();
        Destroy(gameObject);
    }
}
