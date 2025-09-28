using System.Collections;
using UnityEngine;

public class PlayerMovementAnimator : MonoBehaviour
{
    // ��� ����� ����� ����������� ��������
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float positionTolerance = 0.01f;
    [SerializeField] private GameObject _playerVisual;

    private Transform _target;
    private bool _hasTarget = false;

    public bool IsMoving { get; private set; }

    public void Update()
    {
        if (!_hasTarget || _target == null)
        {
            IsMoving = false;
            return;
        }

        // ���������, �������� �� �� ����
        bool atTarget = IsAtPosition(_target.position);

        if (!atTarget)
        {
            if (GlobalFlags.GetFlag(GlobalFlags.Flags.BLOCK_PLAYER_MOVING)) return;
            // �������� � ����
            MoveTo(_target.position);
            IsMoving = true;
        }
        else
        {
            // �������� ����
            _playerVisual.transform.position = _target.position; // ����� ������������� �������
            IsMoving = false;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _hasTarget = target != null;

        if (_hasTarget)
        {
            IsMoving = !IsAtPosition(_target.position);
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        // ������� ��������� ������ ��� ������� (�������������� �������)
        GameObject tempTarget = new GameObject("TempTarget");
        tempTarget.transform.position = targetPosition;
        SetTarget(tempTarget.transform);

        // ���������� ��������� ������ ����� ��������� ����
        StartCoroutine(DestroyTempTargetWhenReached(tempTarget));
    }

    private IEnumerator DestroyTempTargetWhenReached(GameObject tempTarget)
    {
        yield return new WaitUntil(() => !IsMoving);
        if (tempTarget != null)
        {
            Destroy(tempTarget);
        }
    }

    private void MoveTo(Vector3 targetPosition)
    {
        _playerVisual.transform.position = Vector2.Lerp(
            _playerVisual.transform.position,
            targetPosition,
            movementSpeed * Time.deltaTime
        );
    }

    public bool IsAtPosition(Vector3 position)
    {
        return Vector3.Distance(_playerVisual.transform.position, position) <= positionTolerance;
    }

    // ����� ��� ����������� ����������� ��� ��������
    public void TeleportTo(Vector3 position)
    {
        _playerVisual.transform.position = position;
        IsMoving = false;
    }

    // ����� ��� ��������� ��������
    public void StopMovement()
    {
        _target = null;
        _hasTarget = false;
        IsMoving = false;
    }
}