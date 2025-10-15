using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPuzzleGameManager : MonoBehaviour
{
    [SerializeField] AddPiecesToGame TotalPiecesAmount;

    private int totslLevel = 0;
    private int amountOfLevels;
    private int currentLevelPieces = 0;
    private int totalCurrentLevelPieces;

    private void GetToalCurrentLevelPiecesAmount()
    {


        totalCurrentLevelPieces = TotalPiecesAmount.currentLevelPieceAmount; 
    }

    private void IncrementCurrentLevelPieces()
    {
        currentLevelPieces += 1;
    }
    private void GoToZeroCurrentPieces()
    {
        currentLevelPieces = 0; // Для того, что бы нельзя было выиграть, просто кликая первый спрайт
    }
    private void OnEnable()
    {
        BoxPuzzleEventManager.OnRigthSelected += IncrementCurrentLevelPieces;
        BoxPuzzleEventManager.OnReturnToNormalOpacity += GoToZeroCurrentPieces;
    }
    private void OnDisable()
    {
        BoxPuzzleEventManager.OnRigthSelected -= IncrementCurrentLevelPieces;
        BoxPuzzleEventManager.OnReturnToNormalOpacity -= GoToZeroCurrentPieces;
    }
    private void Start()
    {
        amountOfLevels = TotalPiecesAmount.sprites.Length;
        GetToalCurrentLevelPiecesAmount();
    }

   
    private void Check()
    {
            BoxPuzzleEventManager.LevelChange();   
            currentLevelPieces = 0;
            totalCurrentLevelPieces = TotalPiecesAmount.currentLevelPieceAmount;
    }
    private void Update()
    {
        if (currentLevelPieces > totalCurrentLevelPieces)
        {
            Check();
       }
    }

}
