using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] protected Vector2 correctPos { get; set; }


    private Collider2D col;

    private void Awake()
    {
        
        col = GetComponent<Collider2D>();
    }
    public void SetCorrectPos(Vector2 pos)
    {
        correctPos = pos;
        Debug.Log("Correct pos set to: " + correctPos);
    }
    public void CheckCorrect() 
    {
        if (Vector2.Distance(transform.localPosition, correctPos) < 0.05f) 
        {
            transform.localPosition = correctPos;
            PieceInRigthPos();
        }
    }
    public void PieceInRigthPos() 
    {
        EventManager.PuzzleCorrect();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sortingOrder = -1;
        col.enabled = false;
    }


}
