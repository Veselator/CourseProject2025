using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2dANimator : MonoBehaviour
{
    public static Player2dANimator Instance { get; private set; }
    public bool IsFlipped { get; private set; } = false;

    private Rigidbody2D rb;
    [SerializeField] private float flipThreshold = 0.1f; // Минимальная скорость для переворота

    private void Awake()
    {
        if (Instance == null) Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckFlip();
    }

    private void CheckFlip()
    {
        // Проверяем, движется ли персонаж (игнорируем минимальные движения)
        if (Mathf.Abs(rb.velocity.x) < flipThreshold)
            return;

        // Если движется влево и не перевёрнут - переворачиваем
        if (rb.velocity.x < 0 && IsFlipped)
        {
            Flip();
        }
        // Если движется вправо и перевёрнут - переворачиваем обратно
        else if (rb.velocity.x > 0 && !IsFlipped)
        {
            Flip();
        }
    }

    private void Flip()
    {
        IsFlipped = !IsFlipped;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}