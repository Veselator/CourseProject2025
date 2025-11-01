using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private bool _isActive = true;

    [HideInInspector] public Vector3 TeleportPosition { get; private set; }
    [SerializeField] private Transform _teleportPoint;
    [SerializeField] private Teleporter _linkedTeleporter;
    [SerializeField] private float _cooldownTime = 1f;

    private void Start()
    {
        TeleportPosition = _teleportPoint.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isActive) return;

        RigidbodyPlatformerMovement movement = collision.GetComponent<RigidbodyPlatformerMovement>();
        if (movement == null) return;

        if (_linkedTeleporter == null)
        {
            Debug.LogWarning($"“елепорт {gameObject.name} не имеет св€занного телепорта!");
            return;
        }

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.position = _linkedTeleporter.TeleportPosition;
        }
        else
        {
            collision.transform.position = _linkedTeleporter.TeleportPosition;
        }

        StartCooldown();
        _linkedTeleporter.StartCooldown();
    }

    public void StartCooldown()
    {
        if (!_isActive) return;
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        _isActive = false;
        yield return new WaitForSeconds(_cooldownTime);
        _isActive = true;
    }
}