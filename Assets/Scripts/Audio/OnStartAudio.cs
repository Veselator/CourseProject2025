using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartAudio : MonoBehaviour
{
    [SerializeField] private string _audio;
    [SerializeField] private float _minPitch = 0.8f;
    [SerializeField] private float _maxPitch = 1.2f;
    [SerializeField] private float _volumeFactor = 1f;

    void Start()
    {
        GameAudioManager.Instance.PlaySFXWithRandomPitch(_audio, _minPitch, _maxPitch, _volumeFactor);
    }
}
