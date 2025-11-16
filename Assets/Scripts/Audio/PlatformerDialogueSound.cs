using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerDialogueSound : MonoBehaviour
{
    [SerializeField] private DialoguesManager _dialoguesManager;
    [SerializeField] private float _volume = 1f;
    private GameAudioManager _gameAudioManager;

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;
        _dialoguesManager.OnDialogueNodeStarted += HandleDialogueLineStarted;
    }

    private void OnDestroy()
    {
        _dialoguesManager.OnDialogueNodeStarted -= HandleDialogueLineStarted;
    }

    private void HandleDialogueLineStarted(DialogueNodeSO node)
    {
        _gameAudioManager.StopDialogue();
        if (!string.IsNullOrEmpty(node.LinkedAudio))
        {
            _gameAudioManager.PlayDialogue(node.LinkedAudio, _volume);
        }
    }
}
