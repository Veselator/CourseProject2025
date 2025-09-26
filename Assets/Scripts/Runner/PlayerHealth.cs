using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int health = 3;
    public int Health => health;
    public Action OnPlayerHit;
    public Action OnPlayerDied;

    public static PlayerHealth Instance { get; private set; }
    private PlayerLaneController _playerLaneController;
    public bool IsPossibleToHitPlayer { get; set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        IsPossibleToHitPlayer = true;
    }

    private void Start()
    {
        _playerLaneController = PlayerLaneController.Instance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision detected with " + other.gameObject.name);

        if (!IsPossibleToHitPlayer) return;

        if (other.transform.TryGetComponent<ObstacleInfo>(out ObstacleInfo obstacleInfo))
        {
            if (obstacleInfo.LaneIndex != (int)_playerLaneController.CurrentLane) return;
            health--;
            Debug.Log("Player hit an obstacle! Health: " + health);
            OnPlayerHit?.Invoke();
            CameraShake.ShakeCamera?.Invoke();

            if (health <= 0)
            {
                Debug.Log("Player is dead!");
                OnPlayerDied?.Invoke();

                GlobalFlags.SetFlag(GlobalFlags.Flags.GAME_OVER);
                return;
                // Handle player death (e.g., end game, respawn, etc.)
            }
            Destroy(other.gameObject);
        }
    }
}
