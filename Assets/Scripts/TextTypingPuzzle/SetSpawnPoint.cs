using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetSpawnPoint : MonoBehaviour
{
   public static SetSpawnPoint Instance { get; private set; }
    [SerializeField] public RectTransform RightPos;
    [SerializeField] public RectTransform LeftPos;
    [SerializeField] public RectTransform TopPos;
    [SerializeField] private RectTransform Border;
    [SerializeField] public Transform parrants;

    private static readonly Vector2 LEFT_OFFSET = new Vector2(-50, 0);
    private static readonly Vector2 RIGHT_OFFSET = new Vector2(50, 0);
    private static readonly Vector2 UP_OFFSET = new Vector2(0, 200);
    private static readonly float BORDER_WIDTH = 0.05f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void UpdateSpawnPoints()
    {
        RectTransform rectCanvas = parrants.GetComponent<RectTransform>();
        LeftPos.anchoredPosition = new Vector2(-rectCanvas.rect.width / 2, 0) + LEFT_OFFSET;
        RightPos.anchoredPosition = new Vector2(rectCanvas.rect.width / 2, 0) + RIGHT_OFFSET;
        TopPos.anchoredPosition = new Vector2(0, rectCanvas.rect.height / 2) + UP_OFFSET;

        Border.anchoredPosition = Vector2.zero;
        Border.localScale = new Vector2(BORDER_WIDTH, rectCanvas.rect.height);

    }

}
