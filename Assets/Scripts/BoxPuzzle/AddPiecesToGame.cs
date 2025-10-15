using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddPiecesToGame : MonoBehaviour
{
    [SerializeField] public GameObject[] sprites;

    private int currentLevel = 0;
    public int currentLevelPieceAmount { get; private set; }
    GameObject levelInstance;
    private void LoadLevel() 
    {
        if (currentLevel >= sprites.Length)
        {
            Debug.Log("Все уровни пройдены!");
            return;
        }
        levelInstance = Instantiate(sprites[currentLevel]);
       
            Transform[] children = levelInstance.GetComponentsInChildren<Transform>(); // Берем из префаба спрайты
            if (children == null) return;
            foreach (var c in children) 
            {
                if (c == levelInstance.transform) continue; // Добавляем нужные компоненты на спрайты
                Collider2D collider2D = c.GetComponent<Collider2D>();
                if (collider2D == null) 
                { 
                c.gameObject.AddComponent<BoxCollider2D>();
                }
                 BoxPiece bp = c.gameObject.GetComponent<BoxPiece>();
            if (bp == null)
            {
                bp = c.gameObject.AddComponent<BoxPiece>();
            }
            bp.Id = int.Parse(c.name);
            }
        currentLevelPieceAmount = children.Length-2;
        currentLevel++;
        
    }
    private void ClearBeforeChange() 
    {
        
        Destroy(levelInstance);
    }
    private void LevelChange() 
    {
        ClearBeforeChange();
        LoadLevel();
    }
    private void OnEnable()
    {
        BoxPuzzleEventManager.OnLevelChange += LevelChange;

    }
    private void OnDisable()
    {
        BoxPuzzleEventManager.OnLevelChange -= LevelChange;
       
    }
    private void Awake()
    {
        LoadLevel();
    }
}
