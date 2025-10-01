using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;
    public float speed;
    private bool playerDetected = false;
    private Rigidbody2D rb;
    private Base_Enemy health;
    public float Knocking = 0.5f;
   

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Base_Enemy>();

    }
    private void Move_On_Player() 
    {
        if (player != null) 
        {
            var current= Vector2.MoveTowards(transform.position, player.transform.position, speed*Time.deltaTime);
            rb.MovePosition(current);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            playerDetected = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = false;
        }

    }
    private void FixedUpdate()
    {
        if (!health.isEnemyKnocked && playerDetected)
        {
            Move_On_Player();
        }
    }

    }
