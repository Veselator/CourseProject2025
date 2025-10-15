using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleScattering : MonoBehaviour
{
    const float offsetForX = 3f;
    const float offsetForY = 2f;
    [SerializeField] private Box spawnBox;

    public static PuzzleScattering Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ScatterPuzzles(List<PuzzlePiece> pieces) 
    {
        foreach (var p in pieces)
        {
            float scatterRangeX = Random.Range(spawnBox.startPoint.x, spawnBox.endPoint.x);
            float scatterRangeY = Random.Range(spawnBox.startPoint.y, spawnBox.endPoint.y);

            // задаём случайную позицию
            p.transform.position = new Vector3(scatterRangeX, scatterRangeY, 0);
        }
    }
}
