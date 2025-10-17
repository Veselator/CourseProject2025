using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPieceVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _normalColor = Color.gray;
    [SerializeField] private Color _selectedColor = Color.white;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetSelect(false);
    }

    private void SetSelect(bool isSelected)
    {
        if(isSelected) _spriteRenderer.color = _selectedColor;
        else _spriteRenderer.color = _normalColor;
    }

    private void OnMouseEnter()
    {
        SetSelect(true);
    }

    private void OnMouseExit()
    {
        SetSelect(false);
    }
}
