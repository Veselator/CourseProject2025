using UnityEngine;

public class RunnerPlayerAudioEffects : MonoBehaviour
{
    private GameAudioManager _audioManager;
    private JumpTracker _jumpTracker;
    private PlayerHealth _playerHealth;
    private MoneyTracker _moneyTracker;

    [SerializeField] private string _boomSound;
    [SerializeField] private float _boomSoundMinPitch = 0.8f;
    [SerializeField] private float _boomSoundMaxPitch = 1.2f;

    [SerializeField] private string _moneySound;

    [SerializeField] private string _jumpSound;
    [SerializeField] private float _jumpSoundMinPitch = 0.8f;
    [SerializeField] private float _jumpSoundMaxPitch = 1.2f;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;
        _playerHealth = PlayerHealth.Instance;
        _jumpTracker = JumpTracker.Instance;
        _moneyTracker = MoneyTracker.Instance;

        _playerHealth.OnPlayerHit += Boom;
        _moneyTracker.OnMoneyChanged += MoneySound;
        _jumpTracker.OnJumpAnimationStarted += PlayJumpSound;
    }

    private void OnDestroy()
    {
        _playerHealth.OnPlayerHit -= Boom;
        _moneyTracker.OnMoneyChanged -= MoneySound;
        _jumpTracker.OnJumpAnimationStarted -= PlayJumpSound;
    }

    private void MoneySound()
    {
        _audioManager.PlaySound(_moneySound);
    }

    private void PlayJumpSound()
    {
        _audioManager.PlaySFXWithRandomPitch(_jumpSound, _jumpSoundMinPitch, _jumpSoundMaxPitch);
    }

    private void Boom()
    {
        _audioManager.PlaySFXWithRandomPitch(_boomSound, _boomSoundMinPitch, _boomSoundMaxPitch);
    }
}
