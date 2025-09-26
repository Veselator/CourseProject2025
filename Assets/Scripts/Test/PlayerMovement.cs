using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private const int NUM_OF_POSITINS = 3;

    private int currentPositionIndex = 1;
    public int CurrentPositionIndex
    {
        get { return currentPositionIndex; }
        private set { if(value >= 0 && value < NUM_OF_POSITINS) currentPositionIndex = value; }
    }

    [SerializeField] private Transform[] positions = new Transform[NUM_OF_POSITINS];
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GameObject _player;

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 movementVector = _playerInput.GetMovementVector();

        if (movementVector.y < 0)
        {
            CurrentPositionIndex++;
        }

        else if (movementVector.y > 0)
        {
            CurrentPositionIndex--;
        }

        _MoveToPoint();
    }

    private void _MoveToPoint()
    {
        _player.transform.position = Vector2.MoveTowards(_player.transform.position, positions[currentPositionIndex].position, movementSpeed * Time.deltaTime);
    }
}
