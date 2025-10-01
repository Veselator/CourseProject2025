using UnityEngine;

public class StaticYSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform sortingPoint; // ����� ���������� (��������� ������)
    [SerializeField] private float sortingOrderScale = -100f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sortingPoint == null)
            sortingPoint = transform;

        // ��� ��������� �������� ���������� ���������� ���� ���
        UpdateSortingOrder();
    }

    private void UpdateSortingOrder()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(sortingPoint.position.y * sortingOrderScale);
    }
}