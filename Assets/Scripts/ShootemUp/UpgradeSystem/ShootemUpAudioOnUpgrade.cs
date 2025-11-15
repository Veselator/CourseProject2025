using UnityEngine;

public class ShootemUpAudioOnUpgrade : MonoBehaviour
{
    [SerializeField] private UpgradesManager _upgradesManager;
    [SerializeField] private string _soundIfUpgrade;
    [SerializeField] private float _minPitch = 0.9f;
    [SerializeField] private float _maxPitch = 1.1f;

    private GameAudioManager _gameAudioManager;

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;

        _upgradesManager.OnUpgradeChosen += HandleUpgradeChosen;
    }

    private void OnDestroy()
    {
        _upgradesManager.OnUpgradeChosen -= HandleUpgradeChosen;
    }

    private void HandleUpgradeChosen(IUpgrade obj)
    {
        _gameAudioManager.PlaySFXWithRandomPitch(_soundIfUpgrade, _minPitch, _maxPitch);
    }
}
