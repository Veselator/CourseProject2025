using UnityEngine;

public class LasersTurnOffSFX : MonoBehaviour
{
    [SerializeField] private LaserTurnOffer _laserTurnOffer;
    [SerializeField] private string _lasersTurnOffSound;
    private GameAudioManager _gameAudioManager;
    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;
        _laserTurnOffer.OnLasersTurnedOff += HandleOnLasersTurnedOff;
    }
    private void OnDestroy()
    {
        _laserTurnOffer.OnLasersTurnedOff -= HandleOnLasersTurnedOff;
    }
    private void HandleOnLasersTurnedOff()
    {
        _gameAudioManager.PlaySound(_lasersTurnOffSound);
    }
}
