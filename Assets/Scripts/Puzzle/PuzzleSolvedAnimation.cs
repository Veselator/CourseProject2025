using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleSolvedAnimation : MonoBehaviour
{
    private Gm _gm;
    [SerializeField] private GameObject _puzzlePrefab;
    [SerializeField] private GameObject _border;
    private GameObject newImage;

    public static PuzzleSolvedAnimation Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _gm = Gm.Instance;
        _gm.OnPuzzleSolved += StartSolveAnimation;
    }

    private void OnDestroy()
    {
        _gm.OnPuzzleSolved -= StartSolveAnimation;
    }

    private void InitGameObjectByTexture(GameObject newImage, Texture2D tex)
    {
        Rect spriteRect = new Rect(0.0f, 0.0f, tex.width, tex.height);

        Vector2 pivot = new Vector2(0.5f, 0.5f);

        int pieceWidthPx = tex.width / _gm.GridSize.x;

        float pixelsPerUnit = pieceWidthPx / _gm.PieceSize.x;
        newImage.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, spriteRect, pivot, pixelsPerUnit);
    }

    public void StartSolveAnimation()
    {
        _gm.ClearPieces();
        newImage = Instantiate(_puzzlePrefab, _gm.BorderPosition, Quaternion.identity);

        Texture2D CurrentTexture = _gm.CurrentTexture;
        InitGameObjectByTexture(newImage, CurrentTexture);

        newImage.GetComponent<PuzzleSolvedImageScript>().Init(_border);
    }

    public void ResetPuzzleSolvedAnimation()
    {
        Destroy(newImage);
        newImage = null;
    }
}
