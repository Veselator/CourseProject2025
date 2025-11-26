using UnityEngine;
using UnityEngine.UI;

public class CursorToggleSFX : MonoBehaviour
{
    [SerializeField] private string _sfx;
    [SerializeField] private CursorUIToggle _linkedCursorToggle;
    [SerializeField] private float _minPitch = 0.8f;
    [SerializeField] private float _maxPitch = 1.2f;

    private GameAudioManager _gameAudioManager;

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;

        _linkedCursorToggle.OnToggled += PlaySound;
    }

    private void OnDestroy()
    {
        if (_linkedCursorToggle != null) _linkedCursorToggle.OnToggled -= PlaySound;
    }

    private void PlaySound()
    {
        _gameAudioManager.PlaySFXWithRandomPitch(_sfx, _minPitch, _maxPitch);
    }
}
