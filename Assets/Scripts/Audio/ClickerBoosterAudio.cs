using UnityEngine;

public class ClickerBoosterAudio : MonoBehaviour
{
    private BoosterHandler _boosterHandler;
    private GameAudioManager _audioManager;

    [SerializeField] private string _buySound;
    [SerializeField] private string _upgradeSound;
    [SerializeField] private string _failedSound;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;
        _boosterHandler = GetComponent<BoosterHandler>();

        _boosterHandler.OnBoosterBought += PlayBuySound;
        _boosterHandler.OnBoosterUpgraded += PlayUpgradeSound;
        _boosterHandler.OnFailedToDoAction += PlayFailedSound;
    }

    private void OnDestroy()
    {
        if(_boosterHandler == null) return;
        _boosterHandler.OnBoosterBought -= PlayBuySound;
        _boosterHandler.OnBoosterUpgraded -= PlayUpgradeSound;
        _boosterHandler.OnFailedToDoAction -= PlayFailedSound;
    }

    private void PlayBuySound()
    {
        _audioManager.PlaySound(_buySound);
    }

    private void PlayUpgradeSound()
    {
        _audioManager.PlaySound(_upgradeSound);
    }

    private void PlayFailedSound()
    {
        _audioManager.PlaySound(_failedSound);
    }
}
