using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSFX : MonoBehaviour
{
    [SerializeField] private string _onDoorClosedSFX;
    [SerializeField] private string _onDoorOpenedSFX;

    private Door _door;
    private GameAudioManager _audioManager;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;

        _door = GetComponent<Door>();
        _door.OnStateChanged += HandleDoorStateChanged;
    }

    private void OnDestroy()
    {
        if (_door != null)
        {
            _door.OnStateChanged -= HandleDoorStateChanged;
        }
    }

    private void HandleDoorStateChanged(bool isOpen, bool isSilent)
    {
        if(!isSilent) _audioManager.PlaySound(isOpen ? _onDoorOpenedSFX : _onDoorClosedSFX);
    }
}
