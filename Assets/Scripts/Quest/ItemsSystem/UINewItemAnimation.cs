using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UINewItemAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 0.8f);

    [SerializeField] private Vector2 offsetVector = new Vector2(50f, 10f);

    private RectTransform rectTransform;
    private Image image;
    private Canvas canvas;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private GameObject targetInventoryItem;

    public void Init(Vector3 worldStartPosition, GameObject inventoryItem, Canvas uiCanvas, Sprite itemSprite)
    {
        canvas = uiCanvas;
        targetInventoryItem = inventoryItem;

        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        if (image == null)
        {
            image = gameObject.AddComponent<Image>();
        }

        image.sprite = itemSprite;
        image.preserveAspect = true;

        // Конвертируем мировые координаты в локальные координаты Canvas
        startPosition = WorldCoordinates2UICoordinates.Calculate(worldStartPosition, canvas);
        rectTransform.localPosition = startPosition;

        targetInventoryItem.SetActive(false);

        StartCoroutine(AnimateToInventory());
    }

    private IEnumerator AnimateToInventory()
    {
        // Ждём пересчёта layout
        yield return null;
        Canvas.ForceUpdateCanvases();
        yield return null;

        float elapsed = 0f;
        Vector3 initialScale = Vector3.one * 1.5f;

        // Конвертируем позицию целевого предмета в локальные координаты Canvas
        RectTransform targetRect = targetInventoryItem.GetComponent<RectTransform>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Преобразуем из локальных координат ItemsLayout в локальные координаты Canvas
        Vector3 targetWorldPos = targetRect.TransformPoint(Vector3.zero);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            RectTransformUtility.WorldToScreenPoint(null, targetWorldPos),
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 targetLocalPos
        );

        targetPosition = targetLocalPos + offsetVector;

        Debug.Log($"Start Position: {startPosition}");
        Debug.Log($"Target Position: {targetPosition}");
        Debug.Log($"Target World Pos: {targetWorldPos}");

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float curvedT = movementCurve.Evaluate(t);
            rectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, curvedT);

            float scaleT = scaleCurve.Evaluate(t);
            rectTransform.localScale = initialScale * scaleT;

            //if (t > 0.7f)
            //{
            //    float fadeT = (t - 0.7f) / 0.3f;
            //    Color color = image.color;
            //    color.a = 1f - fadeT;
            //    image.color = color;
            //}

            yield return null;
        }

        CompleteAnimation();
    }

    private void CompleteAnimation()
    {
        if (targetInventoryItem != null)
        {
            targetInventoryItem.SetActive(true);
            //StartCoroutine(PopEffect(targetInventoryItem.GetComponent<RectTransform>()));
        }

        Destroy(gameObject);
    }
}