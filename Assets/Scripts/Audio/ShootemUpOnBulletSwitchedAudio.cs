using UnityEngine;

public class ShootemUpOnBulletSwitchedAudio : MonoBehaviour
{
    [SerializeField] private BulletSwitcher _bulletSwitcher;
    [SerializeField] private string _soundIfBulletSwitched;
    [SerializeField] private float _minPitch = 0.9f;
    [SerializeField] private float _maxPitch = 1.1f;

    private GameAudioManager _gameAudioManager;

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;

        _bulletSwitcher.OnBulletSwitched += HandleUpgradeChosen;
    }

    private void OnDestroy()
    {
        _bulletSwitcher.OnBulletSwitched -= HandleUpgradeChosen;
    }

    private void HandleUpgradeChosen(int _)
    {
        _gameAudioManager.PlaySFXWithRandomPitch(_soundIfBulletSwitched, _minPitch, _maxPitch);
    }
}
