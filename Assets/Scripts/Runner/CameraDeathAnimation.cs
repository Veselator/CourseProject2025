using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeathAnimation : MonoBehaviour
{
    private float startCameraSize;
    private float endCameraSize = 2f;
    private float movementDuration = 3f;

    private Camera mainCamera;
    private CameraShake _shake;
    [SerializeField] private GameObject playerTransform;
    private bool isPlayingAnimation = false;

    void Start()
    {
        mainCamera = Camera.main;
        startCameraSize = mainCamera.orthographicSize;

        GlobalFlags.onFlagChanged += CheckGlobalFlags;
        _shake = GetComponent<CameraShake>();
    }

    private void OnDestroy()
    {
        GlobalFlags.onFlagChanged -= CheckGlobalFlags;
    }

    private void CheckGlobalFlags(string flagName, bool flagState)
    {
        if (flagName == Flags.GameOver.ToString())
        {
            PlayDeathAnimation();
        }
    }

    private void PlayDeathAnimation()
    {
        if (isPlayingAnimation) return;
        _shake.enabled = false;
        StartCoroutine(MoveCameraToTarget(playerTransform.transform));
    }

    public IEnumerator MoveCameraToTarget(Transform targetObject)
    {
        isPlayingAnimation = true;
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

        isPlayingAnimation = false;
    }
}
