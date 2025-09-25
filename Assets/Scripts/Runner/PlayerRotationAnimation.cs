using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRotationAnimation : MonoBehaviour
{
    // ��, �������� ��������
    // ������ �������� �� ������� �������� ��� ��������
    // ������ ������ ������� player
    // ����� ����������� ����������� � PlayerPointController

    public static PlayerRotationAnimation Instance;
    
    private SpawnRoad _spawnRoad;
    private float animationDuration = 0.64f;
    private float CameraOffsetX;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _spawnRoad = SpawnRoad.Instance;
        CameraOffsetX = 7.07f;
    }

    public void OnRotateRoad(int turnId)
    {
        Transform center = _spawnRoad.LastSpawnedTurn.transform.Find("Center");
        if (center == null)
        {
            Debug.LogError("PlayerRotationAnimation: Center point not found in the last spawned turn.");
            return;
        }

        StartCoroutine(MoveCoroutine(new Vector2(center.position.x - CameraOffsetX, center.position.y)));
        //if (turnId == 0) StartCoroutine(MoveCoroutine(_spawnRoad.LastSpawnedTurn.transform.position + Vector3.right * 1f));
        //if (turnId == 1) StartCoroutine(MoveCoroutine(_spawnRoad.LastSpawnedTurn.transform.position + Vector3.up * 7f));
    }

    private IEnumerator MoveCoroutine(Vector2 endPosition)
    {
        Vector2 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            transform.position = Vector2.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ����������, ��� ������ ����� ������ �������� �������
        transform.position = endPosition;
    }
}
