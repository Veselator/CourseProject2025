using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public static Player_Attack Instance { get; private set; }

    [Header("��������� �����")]
    public float attackR;
    public float attackTime = 0.5f;
    public int DAMAGE = 35;
    public Transform attackPos;
    public LayerMask WhatIsEnemy;

    [Header("�������� �����")]
    [SerializeField] private float attackAngle = 90f; // ���� ������
    [SerializeField] private float attackSwingDuration = 0.2f; // ������������ ������
    [SerializeField] private float attackReturnDuration = 0.3f; // ������������ ��������
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Player_Rotation playerRotation;

    private float attackTimer = 0;
    private Stamina_Sys s;
    public bool IsAttacking { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        s = GetComponent<Stamina_Sys>();

        // ��������� trail �� ���������
        if (trailRenderer != null)
            trailRenderer.enabled = false;
    }

    private void Do_Attack()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPos.position, attackR, WhatIsEnemy);
        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i] != null)
            {
                Vector2 dir = (enemy[i].transform.position - transform.position);
                enemy[i].GetComponent<IDamagable>().Enemy_Gets_Damage(DAMAGE, dir);
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        IsAttacking = true;

        // �������� trail
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
            trailRenderer.Clear();
        }


        // ��������� ��������� ����
        Quaternion startRotation = playerRotation.transform.localRotation;
        float startAngle = startRotation.eulerAngles.z;

        // ���������� ����������� ����� (��������� ���� � ��������)
        float targetAngle = startAngle - attackAngle;

        // ���� ������
        float elapsed = 0f;
        while (elapsed < attackSwingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / attackSwingDuration;
            float currentAngle = Mathf.Lerp(startAngle, targetAngle, t);
            playerRotation.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }

        // ������� ���� � ������ �����
        Do_Attack();

        // ���� ��������
        elapsed = 0f;
        while (elapsed < attackReturnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / attackReturnDuration;
            float currentAngle = Mathf.Lerp(targetAngle, startAngle, t);
            playerRotation.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }

        // ��������� trail
        if (trailRenderer != null)
            trailRenderer.enabled = false;

        IsAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPos == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackR);
    }

    private void Update()
    {
        if (s.amountOfStamina > 0 && attackTimer <= 0 && Input.GetKeyDown(KeyCode.Mouse0) && !IsAttacking)
        {
            StartCoroutine(AttackCoroutine());
            attackTimer = attackTime;
            s.Take_Stamina(10f);
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }
}