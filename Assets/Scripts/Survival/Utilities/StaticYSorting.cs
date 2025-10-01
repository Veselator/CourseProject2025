using UnityEngine;

public class StaticYSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform sortingPoint; // Точка сортировки (основание пальмы)
    [SerializeField] private float sortingOrderScale = -100f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sortingPoint == null)
            sortingPoint = transform;

        // Для статичных объектов достаточно установить один раз
        UpdateSortingOrder();
    }

    private void UpdateSortingOrder()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(sortingPoint.position.y * sortingOrderScale);
    }
}