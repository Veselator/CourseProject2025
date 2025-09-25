using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    float Speed { get; }
    Vector2 Direction { get; set; }
    bool IsSpawnedByPlayer { get; }

    void Move();
    void Initialize(Vector2 startDirection, float bulletSpeed, bool isSpawnedByPlayer, float damageMultiplayer);
    void Initialize(Vector2 startDirection, bool isSpawnedByPlayer, float damageMultiplayer);
    void Initialize(Vector2 startDirection, bool isSpawnedByPlayer);
}
