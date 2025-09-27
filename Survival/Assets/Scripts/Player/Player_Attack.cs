using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public static Player_Attack Instance { get; private set; }
    public float attackR;
    public float attackTime = 0.5f;
    public int DAMAGE = 35;
    private float attackTimer = 0;
    public Transform attackPos;
    public LayerMask WhatIsEnemy;
    private Stamina_Sys s;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        s = GetComponent<Stamina_Sys>();
    }
    private void Do_Attack() 
    {
       
        
            Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPos.position, attackR, WhatIsEnemy);
            for (int i = 0; i < enemy.Length; i++)
            {
                if (enemy[i] != null)
                {
                    Vector2 dir = (enemy[i].transform.position-transform.position);
                    enemy[i].GetComponent<IDamagable>().Enemy_Gets_Damage(DAMAGE, dir);
                }
            }
        
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPos == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackR);
    }
    private void Update()
    {
        if (s.amountOfStamina >0&&attackTimer <= 0 && Input.GetKeyDown(KeyCode.Space))
        {
            Do_Attack();
            attackTimer = attackTime;
            s.Take_Stamina(10f);
        }
        else 
        {
            attackTimer -= Time.deltaTime;
        }
    }
}
