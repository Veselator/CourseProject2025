using UnityEngine;

public class QuestTimerAudio : MonoBehaviour
{
    [SerializeField] QuestTimerManager _questTimerManager;
    [SerializeField] private float _timeToStartPlayAudio = 8f;
    [SerializeField] private string _soundToPlay;
    private GameAudioManager _audioManager;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;

        _questTimerManager.OnTimerChanged += HandleTimerChanged;
        _questTimerManager.OnTimerReset += HandleTimerResetOrExpired;
        _questTimerManager.OnTimerExpired += HandleTimerResetOrExpired;
    }

    private void OnDestroy()
    {
        _questTimerManager.OnTimerChanged -= HandleTimerChanged;
        _questTimerManager.OnTimerReset -= HandleTimerResetOrExpired;
        _questTimerManager.OnTimerExpired -= HandleTimerResetOrExpired;

        _audioManager.StopLoopingSound(_soundToPlay);
    }

    private void HandleTimerChanged(float time)
    {
        if (time < _timeToStartPlayAudio && !_audioManager.IsLoopingSoundPlaying(_soundToPlay)) _audioManager.PlayLoopingSound(_soundToPlay);
    }

    private void HandleTimerResetOrExpired()
    {
        _audioManager.StopLoopingSound(_soundToPlay);
    }
}
