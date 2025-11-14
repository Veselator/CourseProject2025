using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AudioLevelControllHandler : MonoBehaviour
{
    [SerializeField] private Slider _linkedSlider;
    [SerializeField] private AudioSettingsCategory _audioSettingsCategory;

    private GameAudioManager _audioManager;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;

        _linkedSlider.onValueChanged.AddListener(HandleValueChanged);

        LinkSliderValueToActualLevelOfSound();
    }

    private void LinkSliderValueToActualLevelOfSound()
    {
        switch (_audioSettingsCategory)
        {
            case AudioSettingsCategory.Master:
                _linkedSlider.value = _audioManager.GetMasterVolume();
                break;
            case AudioSettingsCategory.Music:
                _linkedSlider.value = _audioManager.GetMusicVolume();
                break;
            case AudioSettingsCategory.SFX:
                _linkedSlider.value = _audioManager.GetSFXVolume();
                break;
            case AudioSettingsCategory.Dialogue:
                _linkedSlider.value = _audioManager.GetDialogueVolume();
                break;
        }
    }

    private void OnDestroy()
    {
        _linkedSlider.onValueChanged.RemoveListener(HandleValueChanged);
    }

    private void HandleValueChanged(float value)
    {
        switch (_audioSettingsCategory)
        {
            case AudioSettingsCategory.Master:
                _audioManager.SetMasterVolume(value);
                break;
            case AudioSettingsCategory.Music:
                _audioManager.SetMusicVolume(value);
                break;
            case AudioSettingsCategory.SFX:
                _audioManager.SetSFXVolume(value);
                break;
            case AudioSettingsCategory.Dialogue:
                _audioManager.SetDialogueVolume(value);
                break;
        }
    }
}

public enum AudioSettingsCategory
{
    Master,
    Music,
    SFX,
    Dialogue
}