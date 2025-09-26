using UnityEngine;

public class ShipRotatorAccordingVelocity : MonoBehaviour
{
    [Header("Target Object")]
    [SerializeField] private GameObject objectToRotate;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationAmplitude = 30f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxSpeedForMaxRotation = 5f;

    [Header("Movement Settings")]
    [SerializeField] private float minMovementThreshold = 0.01f;
    [SerializeField] private bool useXMovement = false;
    [SerializeField] private bool invertRotation = false;

    private Rigidbody2D rb;
    private Transform rotateTransform;

    private Vector2 currentPosition;
    private Vector2 lastPosition;
    private float targetRotation;
    private float currentRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError($"ShipRotatorAccordingVelocity: Rigidbody2D not found on {gameObject.name}");
            enabled = false;
            return;
        }

        if (objectToRotate == null)
        {
            objectToRotate = gameObject;
        }

        rotateTransform = objectToRotate.transform;

        // �������������� �������
        currentPosition = lastPosition = rb.position;
    }

    private void FixedUpdate()
    {
        // ��������� ������������ ������� � FixedUpdate ��� ������������� � �������
        lastPosition = currentPosition;
        currentPosition = rb.position;
    }

    private void Update()
    {
        if (rotateTransform == null) return;

        CalculateTargetRotation();
        ApplyRotation();
    }

    private void CalculateTargetRotation()
    {
        // ��������� �������� �� ����
        Vector2 deltaMovement = currentPosition - lastPosition;

        if (deltaMovement.magnitude < minMovementThreshold)
        {
            targetRotation = 0f;
            return;
        }

        // �������� ��������� ��������
        float movementComponent = useXMovement ? deltaMovement.x : deltaMovement.y;

        // ������������ � "��������" (�������� �� �������)
        float velocity = movementComponent / Time.fixedDeltaTime;

        // ����������� � ��������� [-1, 1]
        float normalizedVelocity = Mathf.Clamp(velocity / maxSpeedForMaxRotation, -1f, 1f);

        // ��������� ������� ���� ��������
        targetRotation = normalizedVelocity * rotationAmplitude;

        if (invertRotation)
        {
            targetRotation = -targetRotation;
        }
    }

    private void ApplyRotation()
    {
        currentRotation = Mathf.LerpAngle(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        rotateTransform.rotation = Quaternion.Euler(rotateTransform.rotation.x, currentRotation, rotateTransform.rotation.y);
    }

    public void SetObjectToRotate(GameObject obj)
    {
        objectToRotate = obj;
        rotateTransform = obj?.transform;
    }
}