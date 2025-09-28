using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRotationAnimation : MonoBehaviour
{
    // Да, название странное
    // Скрипт отвечает за плавную анимацию при повороте
    // Причём именно объекта player
    // Точки перемещения управляются в PlayerPointController

    public static PlayerRotationAnimation Instance;
    
    private SpawnRoad _spawnRoad;
    private float CameraOffsetX;

    [SerializeField] private float fault;
    [SerializeField] private float lerpFactor;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _spawnRoad = SpawnRoad.Instance;
        CameraOffsetX = 7.07f;
    }

    public void OnRotateRoad()
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
        //Vector2 startPosition = transform.position;
        //float elapsedTime = 0f;

        while (Vector2.Distance(transform.position, endPosition) > fault)
        {
            //float t = elapsedTime / animationDuration;
            transform.position = Vector2.Lerp(transform.position, endPosition, lerpFactor * Time.deltaTime);
            //elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Убеждаемся, что объект точно достиг конечной позиции
        transform.position = endPosition;
    }
}
