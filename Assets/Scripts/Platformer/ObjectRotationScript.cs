using UnityEngine;

public class ObjectRotationScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200f;

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
