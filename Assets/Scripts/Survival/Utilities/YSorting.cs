using UnityEngine;

public class YSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool useYPosition = true;
    [SerializeField] private float sortingOrderScale = -100f; // Множитель для точности
    [SerializeField] private float sortingOffset;
    [SerializeField] private float sortingOffsetY;
    private int baseOrder;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseOrder = spriteRenderer.sortingOrder;
    }

    private void LateUpdate()
    {
        // Преобразуем Y-позицию в Order in Layer
        // Чем ниже объект, тем больше Order (рисуется поверх)
        spriteRenderer.sortingOrder = Mathf.RoundToInt((transform.position.y + sortingOffsetY) * sortingOrderScale + sortingOffset) + baseOrder;
    }
}