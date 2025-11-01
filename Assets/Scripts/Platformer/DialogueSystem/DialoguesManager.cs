using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialoguesManager : MonoBehaviour
{
    // Мененджер диалогов для платформера
    // Отвечает за связь реплик
    // Посылает другим компонентам команды что делать
    // Фактически, босс системы диалогов

    [SerializeField] private DialogueTextWriter _textWriter;
    [SerializeField] private GameObject _dialogueRoot;
    [SerializeField] private NextAvailabilityAnimation _nextAvailabilityAnimation;

    // Клавиша для начала следующего диалога
    [SerializeField] private InputActionReference KeyToStartNextNode;

    // UI
    [SerializeField] private TMP_Text _characterTitle;
    [SerializeField] private Image _characterImage;

    private DialogueSO _currentDialogue;
    private bool _isReadyToNextNode = false;
    private int currentNodeIndex = 0;

    public event Action<DialogueSO> OnDialogueEnded;
    private void Awake()
    {
        _textWriter.OnWritingEnded += HandleWritingEnded;
        _dialogueRoot.SetActive(false);

        // Для обработки input
        KeyToStartNextNode.action.performed += NextNodeHanlder;
        KeyToStartNextNode.action.Enable();
    }

    private void OnDestroy()
    {
        _textWriter.OnWritingEnded -= HandleWritingEnded;

        KeyToStartNextNode.action.performed -= NextNodeHanlder;
        KeyToStartNextNode.action.Disable();
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        _currentDialogue = dialogue;
        currentNodeIndex = 0;

        DialogueNodeSO nextNode = _currentDialogue.Nodes[currentNodeIndex];
        UpdateUI(nextNode);
        _textWriter.Write(nextNode);
        _dialogueRoot.SetActive(true);
    }

    // Для привязки к нажатию кнопки
    private void NextNodeHanlder(InputAction.CallbackContext context)
    {
        NextNode();
    }

    private void UpdateUI(DialogueNodeSO currentNode)
    {
        _characterTitle.text = currentNode.Character.Name;
        _characterImage.sprite = currentNode.Character.Photo;
    }

    public void NextNode()
    {
        if (!_isReadyToNextNode) return;

        currentNodeIndex++;
        if (currentNodeIndex >= _currentDialogue.Nodes.Length)
        {
            // Диалог закончился
            _dialogueRoot.SetActive(false);
            OnDialogueEnded?.Invoke(_currentDialogue);
            return;
        }

        _isReadyToNextNode = false;
        _nextAvailabilityAnimation.Hide();
        DialogueNodeSO nextNode = _currentDialogue.Nodes[currentNodeIndex];

        UpdateUI(nextNode);
        _textWriter.Write(nextNode);
    }

    private void HandleWritingEnded()
    {
        _isReadyToNextNode = true;
        _nextAvailabilityAnimation.PlayAnimation();
    }
}
