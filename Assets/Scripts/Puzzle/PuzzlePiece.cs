using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] protected Vector2 correctPos { get; set; }
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float snapThreshold = 0.1f;

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        particles = GetComponent<ParticleSystem>();
    }

    public void SetCorrectPos(Vector2 pos)
    {
        correctPos = pos;
        Debug.Log("Correct pos set to: " + correctPos);
    }
    public void CheckCorrect() 
    {
        if (Vector2.Distance(transform.localPosition, correctPos) < snapThreshold) 
        {
            transform.localPosition = correctPos;
            particles.Play();
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
