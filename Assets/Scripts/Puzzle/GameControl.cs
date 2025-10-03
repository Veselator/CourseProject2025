using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    private Collider2D hit = null;
    LayerMask puzzleLayer;
    int sortingLevel = 0; // Для того, что бы паззлы последовательно друг на друга налаживались при перетаскивании
    private Vector2 offset;

    private void Awake()
    {
        puzzleLayer = LayerMask.GetMask("Puzzle");
    }

    private void Control() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldMousePos = GetMousePos();
            worldMousePos.z = 0f;

             hit = Physics2D.OverlapCircle(worldMousePos, 0.01f, puzzleLayer);
            if (hit)
            {
                IncreaseSortingLevel(hit);
                offset = hit.transform.position - worldMousePos;
            }
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            if (hit)
            {
                
                hit.GetComponent<PuzzlePiece>().CheckCorrect();
              
                
            }
           
            hit = null;
           
        }
        if (hit) 
        {
            Vector3 targetPos = GetMousePos() + offset;
            targetPos.z = 0f;

            hit.transform.position = Vector3.Lerp(hit.transform.position, targetPos,50f* Time.deltaTime);
        }
      
    }
    private Vector2 GetMousePos() 
    {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMousePos.z = 0f;
        return worldMousePos;
    }
    private void IncreaseSortingLevel(Collider2D col) 
    {
        if (sortingLevel >= 100) 
        { 
        sortingLevel = 0;
        }
     col.GetComponent<SpriteRenderer>().sortingOrder = sortingLevel++;
      
    }
    private void Update()
    {
        Control();
    }
}
