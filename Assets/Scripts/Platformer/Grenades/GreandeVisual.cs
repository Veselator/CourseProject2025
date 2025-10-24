using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreandeVisual : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        transform.root.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
