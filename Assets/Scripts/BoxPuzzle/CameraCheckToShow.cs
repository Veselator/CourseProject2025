using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCheckToShow : MonoBehaviour
{
    [SerializeField] private GameObject _miniCameraPanel;

    private void Start()
    {
        _miniCameraPanel.SetActive(false);
        GlobalFlags.onFlagChangedEnum += CheckGlobalFlag;
    }

    private void OnDestroy()
    {
        GlobalFlags.onFlagChangedEnum -= CheckGlobalFlag;
    }

    private void CheckGlobalFlag(Flags flag, bool state)
    {
        if (flag == Flags.IsReadyToShowMiniCamera && !state)
        {
            if (_miniCameraPanel.activeInHierarchy) _miniCameraPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<BoxPiece>(out _)) return;
        if (!GlobalFlags.GetFlag(Flags.IsReadyToShowMiniCamera)) return;

        _miniCameraPanel.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<BoxPiece>(out _)) return;

        if(_miniCameraPanel.activeInHierarchy) _miniCameraPanel.SetActive(false);
    }
}
