using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public static Player_Attack Instance { get; private set; }

    [Header("Настройки атаки")]
    public float attackR;
    public float attackTime = 0.5f;
    public int DAMAGE = 35;
    public Transform attackPos;
    public LayerMask WhatIsEnemy;

    [Header("Анимация атаки")]
    [SerializeField] private float attackAngle = 90f; // Угол замаха
    [SerializeField] private float attackSwingDuration = 0.2f; // Длительность замаха
    [SerializeField] private float attackReturnDuration = 0.3f; // Длительность возврата
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

        // Отключаем trail по умолчанию
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

        // Включаем trail
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
            trailRenderer.Clear();
        }


        // Сохраняем начальный угол
        Quaternion startRotation = playerRotation.transform.localRotation;
        float startAngle = startRotation.eulerAngles.z;

        // Определяем направление атаки (добавляем угол к текущему)
        float targetAngle = startAngle - attackAngle;

        // Фаза замаха
        float elapsed = 0f;
        while (elapsed < attackSwingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / attackSwingDuration;
            float currentAngle = Mathf.Lerp(startAngle, targetAngle, t);
            playerRotation.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }

        // Наносим урон в момент удара
        Do_Attack();

        // Фаза возврата
        elapsed = 0f;
        while (elapsed < attackReturnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / attackReturnDuration;
            float currentAngle = Mathf.Lerp(targetAngle, startAngle, t);
            playerRotation.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }

        // Отключаем trail
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