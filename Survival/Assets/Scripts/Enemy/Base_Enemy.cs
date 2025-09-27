using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Base_Enemy : MonoBehaviour, IDamagable
{
    public int amountOfHealth = 100;
    public float knockBackForce = 5f;
    public bool isEnemyKnocked = false;
    private float knockbackTimer = 0f;
    private Vector2 knockbackDir;
    public int Health => amountOfHealth;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Enemy_Gets_Damage(int damage, Vector2 knockDir)
    {
        amountOfHealth -= damage;
        isEnemyKnocked = true;
        knockbackDir = knockDir.normalized;
        knockbackTimer = 0.2f;
        rb.velocity = knockbackDir * knockBackForce;
        if (amountOfHealth <= 0)
        {
            OnDeath();
            Destroy(gameObject);
        }

    }
    public virtual void OnDeath() 
    {
        KillCounter.Instance.count++;
    }
    private void FixedUpdate()
    {
        if (isEnemyKnocked)
        {
            if (knockbackTimer > 0)
            {
                rb.velocity = knockbackDir * knockBackForce;
                knockbackTimer -= Time.fixedDeltaTime;
            }
            else
            {
                isEnemyKnocked = false;
            }
        }
    }
  
}


