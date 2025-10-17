using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCheckToShow : MonoBehaviour
{
    [SerializeField] private GameObject _miniCameraPanel;

    private void Start()
    {
        _miniCameraPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<BoxPiece>(out _)) return;

        _miniCameraPanel.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<BoxPiece>(out _)) return;

        _miniCameraPanel.SetActive(false);
    }
}
