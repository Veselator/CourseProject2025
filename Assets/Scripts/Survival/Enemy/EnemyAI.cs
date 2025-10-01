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

    [Header("��������")]
    private Animator animator;
    [SerializeField] private float attackDistance = 1.5f; // ��������� ��� �����
    [SerializeField] private float attackCooldown = 1f; // ������� ����� �������
    private float attackTimer = 0f;
    private bool isAttacking = false;

    // ����� ��� ��������� (����� �������� ��������)
    private readonly int isWalkingHash = Animator.StringToHash("IsWalking");
    private readonly int attackHash = Animator.StringToHash("Attack");

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Base_Enemy>();
        animator = GetComponent<Animator>();
    }

    private void Move_On_Player()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            // ���� ���������� ������ ��� ����� - �������, ����� ���������
            if (distanceToPlayer <= attackDistance)
            {
                // ��������������� � �������
                animator.SetBool(isWalkingHash, false);
                TryAttack();
            }
            else
            {
                // ��� � ������
                animator.SetBool(isWalkingHash, true);
                var current = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                rb.MovePosition(current);
            }
        }
    }

    private void TryAttack()
    {
        if (attackTimer <= 0f && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger(attackHash);
            attackTimer = attackCooldown;

            // ���������� ���� ����� ��������� �������� (������������ �������� �����)
            StartCoroutine(ResetAttackFlag());
        }
    }

    private IEnumerator ResetAttackFlag()
    {
        // ��� �������� ������������ �������� �����
        yield return new WaitForSeconds(0.19f);
        isAttacking = false;
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
            animator.SetBool(isWalkingHash, false);
        }
    }

    private void Update()
    {
        // ��������� ������ �����
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!health.isEnemyKnocked && playerDetected)
        {
            Move_On_Player();
        }
        else
        {
            // ���� ���� �� ��������� - ��������� �������� ������
            animator.SetBool(isWalkingHash, false);
        }
    }
}