using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleScattering : MonoBehaviour
{
    const float offsetForX = 3f;
    const float offsetForY = 2f;
    public static void ScatterPuzzles(List<PuzzlePiece> pieces, Transform gameHolder) 
    {
        float sizeX = gameHolder.GetComponent<SpriteRenderer>().bounds.size.x;
        float sizeY = gameHolder.GetComponent<SpriteRenderer>().bounds.size.y;


        foreach (var p in pieces)
        {
            float scatterRangeX = Random.Range(offsetForX / sizeX, 1f / sizeX);   // от середины до правого края
            float scatterRangeY = Random.Range(-offsetForY / sizeY, offsetForY / sizeY);       // по вертикали от -Y до +Y

            // задаём случайную позицию
            p.transform.localPosition = new Vector3(scatterRangeX, scatterRangeY, 0);
        }
    }
}
