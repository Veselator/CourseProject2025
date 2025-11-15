using UnityEngine;

public class ShootemUpOnBulletSwitchedAudio : MonoBehaviour
{
    [SerializeField] private BulletSwitcher _bulletSwitcher;
    [SerializeField] private string _soundIfBulletSwitched;
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
        _gameAudioManager.PlaySound(_soundIfBulletSwitched);
    }
}
