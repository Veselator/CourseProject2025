using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeathAnimation : MonoBehaviour
{
    private float startCameraSize;
    private float endCameraSize = 2f;
    private float movementDuration = 3f;

    private Camera mainCamera;
    private PlayerHealth _playerHealth;
    private CameraShake _shake;

    void Start()
    {
        mainCamera = Camera.main;
        startCameraSize = mainCamera.orthographicSize;

        _playerHealth = PlayerHealth.Instance;
        _playerHealth.OnPlayerDied += PlayDeathAnimation;
        _shake = GetComponent<CameraShake>();
    }

    private void OnDestroy()
    {
        _playerHealth.OnPlayerDied -= PlayDeathAnimation;
    }

    private void PlayDeathAnimation()
    {
        _shake.enabled = false;
        StartCoroutine(MoveCameraToTarget(_playerHealth.transform));
    }

    public IEnumerator MoveCameraToTarget(Transform targetObject)
    {
        Vector3 startPosition = mainCamera.transform.position;
        Vector3 endPosition = new Vector3(targetObject.position.x, targetObject.position.y, startPosition.z);

        float elapsedTime = 0f;

        while (elapsedTime < movementDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / movementDuration;

            // ������� ������������ � �������������� Smoothstep ��� ����� ������������� ��������
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            // ����������� ������
            mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, smoothProgress);

            // ��������� ������� ������
            mainCamera.orthographicSize = Mathf.Lerp(startCameraSize, endCameraSize, smoothProgress);

            yield return null;
        }

        // ����������� ������ ���������� �������� ��������
        mainCamera.transform.position = endPosition;
        mainCamera.orthographicSize = endCameraSize;
    }
}
