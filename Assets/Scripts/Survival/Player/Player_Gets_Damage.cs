using System;
using UnityEngine;

public class Player_Gets_Damage : MonoBehaviour
{
    public Game_Manager manager;
    public int damage;
    public float damageInterval = 0.5f;
    private float damageTimer = 0f;

    public event Action OnPlayerDamage;

    private void Update()
    {
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    private void Game_Over()
    {
        gameObject.SetActive(false);
        GlobalFlags.SetFlag(Flags.GameOver);
        manager.Game_Over();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckDamage(collision);
    }

    private void CheckDamage(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && damageTimer <= 0f)
        {
            Health_System.Instance.Take_Damage(damage);
            OnPlayerDamage?.Invoke();
            damageTimer = damageInterval;

            if (Health_System.Instance.Health <= 0)
            {
                Game_Over();
            }
        }
    }
}