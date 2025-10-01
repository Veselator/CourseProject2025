using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorPicker : MonoBehaviour
{
    // Скрипт, который задаёт случайный цвет объекту
    private void Awake()
    {
        SetRandomColor();
    }

    private void SetRandomColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) Debug.Log("Oh shit, i`m sorry");

        spriteRenderer.color = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
    }
}
