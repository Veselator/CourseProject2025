using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMovingObject : MonoBehaviour
{
    private SpaceSpeedController controller;
    [SerializeField] private float moveFactor;

    private void Start()
    {
        controller = SpaceSpeedController.Instance;
    }

    private void Update()
    {
        HandleMoving();
        CheckEdge();
    }

    private void HandleMoving()
    {
        transform.position += controller.Speed * Time.deltaTime * moveFactor * Vector3.down;
    }

    private void CheckEdge()
    {
        if (transform.position.y < controller.BottomY) transform.position = new Vector3(transform.position.x, controller.TopY, transform.position.z);
    }
}
