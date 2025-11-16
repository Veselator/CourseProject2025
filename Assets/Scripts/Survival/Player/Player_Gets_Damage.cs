using System;
using UnityEngine;

public class Player_Gets_Damage : MonoBehaviour
{
    public Game_Manager manager;
    public int damage;
    public float damageInterval = 0.5f; 
    private float damageTimer = 0f;

    public event Action OnPlayerDamage;

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

    private void CheckDamage(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Health_System.Instance.Take_Damage(damage);
            OnPlayerDamage?.Invoke();
            if (Health_System.Instance.Health <= 0)
            {
                Game_Over();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckStayDamage(collision);
    }

    private void CheckStayDamage(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            damageTimer += Time.deltaTime;
            if (damageTimer > damageInterval)
            {
                Health_System.Instance.Take_Damage(damage);
                if (Health_System.Instance.Health <= 0)
                {

                    Game_Over();
                }
                damageTimer = 0f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        damageTimer = 0f;
    }

}
