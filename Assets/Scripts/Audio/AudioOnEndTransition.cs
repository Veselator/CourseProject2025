using UnityEngine;

public class AudioOnEndTransition : MonoBehaviour
{
    private GameAudioManager _audioManager;
    [SerializeField] private string itsFailBro; // Really cool name

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;
        GlobalFlags.onFlagChangedEnum += CheckFlag;
    }

    private void OnDestroy()
    {
        GlobalFlags.onFlagChangedEnum -= CheckFlag;
    }

    private void CheckFlag(Flags flag, bool state)
    {
        if (flag == Flags.GameOver && state)
        {
            _audioManager.StopAmbient();
            _audioManager.PlayMusic(itsFailBro);
        }
    }
}
