using Unity.VisualScripting;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _standartColor = Color.white;
    [SerializeField] private Color _highlightedColor = Color.black;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = _standartColor;
    }

    public void SetHighlighted(bool state)
    {
        if(state) _spriteRenderer.color = _highlightedColor;
        else _spriteRenderer.color = _standartColor;
    }

    private void OnDisable()
    {
        SetHighlighted(false);
    }
}
