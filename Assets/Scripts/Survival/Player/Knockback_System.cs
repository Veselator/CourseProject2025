using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knockback_System : MonoBehaviour
{
    public new string tag;
    private Rigidbody2D rb;
    public float knockBackForce;
    public float knockBackDuration = 0.2f;
    public float knockBackTimer = 0f;
    public bool isKnockBack = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(tag))
        {
            isKnockBack = true;
            knockBackTimer = knockBackDuration;

            Vector2 knockDir = (transform.position - collision.transform.position).normalized;
            rb.velocity = knockDir * knockBackForce;
        }
    }
 
    private void Update()
    {
        if (isKnockBack) 
        { 
            knockBackTimer -= Time.deltaTime;
            if (knockBackTimer <= 0) 
            { 
            isKnockBack=false;
             rb.velocity = Vector2.zero;
            }
        }
    }


}
