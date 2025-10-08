using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaclgroundClickerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform leftestPoint;
    [SerializeField] private Transform rightestPoint;

    private void Update()
    {
        HandleMoving();
        CheckEdge();
    }

    private void HandleMoving()
    {
        transform.position += speed * Time.deltaTime * Vector3.left;
    }

    private void CheckEdge()
    {
        if (transform.position.x < leftestPoint.position.x) transform.position = rightestPoint.position;
    }
}
