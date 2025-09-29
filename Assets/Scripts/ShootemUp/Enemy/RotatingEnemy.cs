using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingEnemy : BaseEnemy
{
    [SerializeField] private float rotatingSpeed = 1f;
    private BulletSpawner _bulletSpawner;

    protected override void Start()
    {
        base.Start();
        _bulletSpawner = GetComponent<BulletSpawner>(); // ����������� ������ ����
    }

    protected override void Update()
    {
        base.Update();
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(0, 0, rotatingSpeed * Time.deltaTime);
        Debug.Log($"Currently trying to rotate {transform.rotation.z}");
        _bulletSpawner.direction = QuaternionToVector2(transform.rotation);
    }

    private static Vector2 QuaternionToVector2(Quaternion rotation)
    {
        Vector3 forward = rotation * Vector3.right; // ��� Vector3.up, ������� �� ����������
        return new Vector2(forward.x, forward.y).normalized;
    }

}
