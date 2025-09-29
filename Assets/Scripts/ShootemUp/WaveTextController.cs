using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveTextController : MonoBehaviour
{
    [SerializeField] private Text wavesText;
    private Animation waveTextAnimation;
    private WavesManager _wavesManager;

    private void Start()
    {
        _wavesManager = WavesManager.Instance;
        waveTextAnimation = wavesText.gameObject.GetComponent<Animation>();

        _wavesManager.OnWaveStarted += PlayWavesAnimation;
    }

    private void OnDisable()
    {
        _wavesManager.OnWaveStarted -= PlayWavesAnimation;
    }

    private void PlayWavesAnimation(int waveNumber)
    {
        Debug.Log($"PlayWavesAnimation {waveNumber}");
        wavesText.text = $"����� {waveNumber + 1}";
        waveTextAnimation.Play();
    }
}
