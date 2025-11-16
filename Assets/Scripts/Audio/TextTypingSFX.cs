using UnityEngine;

public class TextTypingSFX : MonoBehaviour
{
    [SerializeField] private TypingGameplay _typingGameplay;
    private GameAudioManager _audioManager;

    [SerializeField] private SoundWithRandomPitchSettings _soundWhenCharacterCorrectTyped;
    [SerializeField] private SoundWithRandomPitchSettings _soundWhenChatacyerWrongTyped;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;

        _typingGameplay.OnCharacterCorrectTyped += HandleCharacterCorrectTyped;
        _typingGameplay.OnCharacterIncorrectTyped += HandleCharacterIncorrectTyped;
    }

    private void OnDestroy()
    {
        _typingGameplay.OnCharacterCorrectTyped -= HandleCharacterCorrectTyped;
        _typingGameplay.OnCharacterIncorrectTyped -= HandleCharacterIncorrectTyped;
    }

    private void HandleCharacterCorrectTyped(int _, char _c)
    {
        _audioManager.PlaySFXWithRandomPitch(_soundWhenCharacterCorrectTyped);
    }

    private void HandleCharacterIncorrectTyped(int _, char _c)
    {
        _audioManager.PlaySFXWithRandomPitch(_soundWhenChatacyerWrongTyped);
    }
}
