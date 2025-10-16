using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPiece : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private int orderIndex;
    [SerializeField] private bool isConnected;
    
    private SpriteRenderer spriteRenderer;
    private Collider2D pieceCollider;
    private Vector3 originalScale;
    
    public int Id 
    { 
        get => id; 
        set => id = value; 
    }
    
    public int OrderIndex => orderIndex;
    public bool IsConnected 
    { 
        get => isConnected;
        set => isConnected = value;
    }
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pieceCollider = GetComponent<Collider2D>();
        originalScale = transform.localScale;
    }
    
    public void Initialize(int index)
    {
        orderIndex = index;
        isConnected = false;
    }
    
    public void Highlight()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.8f, 0.8f, 1f, 1f);
            transform.localScale = originalScale * 1.1f;
        }
    }
    
    public void ResetVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
            transform.localScale = originalScale;
        }
    }
    
    public void ShowConnected()
    {
        isConnected = true;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.5f, 1f, 0.5f, 1f);
        }
    }
}
