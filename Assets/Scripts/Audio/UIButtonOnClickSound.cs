using UnityEngine;
using UnityEngine.UI;

public class UIButtonOnClickSound : MonoBehaviour
{
    [SerializeField] private string _sfx;
    [SerializeField] private Button _linkedButton;
    [SerializeField] private float _minPitch = 0.8f;
    [SerializeField] private float _maxPitch = 1.2f;

    private GameAudioManager _gameAudioManager;

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;

        if(_linkedButton == null)
        {
            _linkedButton = GetComponent<Button>();
        }

        _linkedButton.onClick.AddListener(PlaySound);
    }

    private void OnDestroy()
    {
        if(_linkedButton != null) _linkedButton.onClick.RemoveListener(PlaySound);
    }

    private void PlaySound()
    {
        _gameAudioManager.PlaySFXWithRandomPitch(_sfx, _minPitch, _maxPitch);
    }
}
