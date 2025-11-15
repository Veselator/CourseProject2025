using UnityEngine;

public class ShootemUpAudioOnUpgrade : MonoBehaviour
{
    [SerializeField] private UpgradesManager _upgradesManager;
    [SerializeField] private string _soundIfUpgrade;
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
        _gameAudioManager.PlaySound(_soundIfUpgrade);
    }
}
