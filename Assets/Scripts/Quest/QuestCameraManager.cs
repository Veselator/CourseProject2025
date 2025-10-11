using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestCameraManager : MonoBehaviour
{
    [SerializeField] private GameObject CameraPointsHolder;
    [SerializeField] private Vector3 cameraOffset;
    private Transform[] cameraPoints;

    private void Start()
    {
        InitCameraPoints();
        MoveCamera2Point(0);
    }

    public void MoveCamera2Point(int pointId)
    {
        transform.position = cameraPoints[pointId].position + cameraOffset;
    }

    private void InitCameraPoints()
    {
        cameraPoints = CameraPointsHolder.GetComponentsInChildren<Transform>().Where(t => t != CameraPointsHolder.transform).ToArray();
    }
}
