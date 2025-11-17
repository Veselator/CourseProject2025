using System.Collections;
using UnityEngine;

public class PlatformerCutscenesManager : MonoBehaviour
{
    // Мененджер кат-сцен для платформера
    // Может быть адаптирован не только под платформер
    // При налчии соотвествующих интерфейсов

    // Ссылки на нужные компоненты

    [SerializeField] private PlayerPlatformerHandler _playerPlatformerHandler;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private CameraZoomByMouse _cameraZoom;
    [SerializeField] private DialoguesManager _dialoguesManager;
    [SerializeField] private BossPhasesManager _bossPhasesManager;
    [SerializeField] private LivesManager _livesManager;
    [SerializeField] private ObjectMoverService _objectMover;
    [SerializeField] private BossAnimationController _bossAnimationController;
    private GameAudioManager _gameAudioManager;

    // Внутренние поля
    private PlatformerCutscene _currentCutscene;
    private CutsceneAction[] _currentActions;
    private int _currentActionPointer; // Указывает на действие, которое ещё не выполнено
    // Внутренняя переменная для очистки мусора
    private bool _isSubscribedToDialoguesManager = false;

    public bool IsPlayingCutscene => _currentCutscene != null;

    public static PlatformerCutscenesManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;
    }

    public void StartCutscene(PlatformerCutscene cutscene)
    {
        // Если другая кат-сцена уже играет - не прерываем
        if (IsPlayingCutscene) return;
        if (GlobalFlags.GetFlag(Flags.GameOver)) return;
        _currentCutscene = cutscene;
        _currentActions = _currentCutscene.Actions;

        // Запускаем процесс
        ParseCurrentAction();
    }

    private void StopCutscene()
    {
        _currentCutscene = null;
        _currentActions = null;
        _currentActionPointer = 0;
    }

    private void ParseCurrentAction()
    {
        // Парсим функцию
        if (GlobalFlags.GetFlag(Flags.GameOver)) return;

        if (_currentActionPointer >= _currentActions.Length)
        {
            StopCutscene();
            return;
        }

        CutsceneAction currentAction = _currentActions[_currentActionPointer];
        switch (currentAction.CAT)
        {
            // Движение
            case CutsceneActionType.BlockMovement:
                _playerPlatformerHandler.IsMovementBlocked = true;
                break;

            case CutsceneActionType.UnblockMovement:
                _playerPlatformerHandler.IsMovementBlocked = false;
                break;

            case CutsceneActionType.ToggleMovement:
                _playerPlatformerHandler.IsMovementBlocked = !_playerPlatformerHandler.IsMovementBlocked;
                break;

            // Камера
            case CutsceneActionType.SetCameraTrackingObject:
                GameObject newTrackingObject = GameObject.Find(currentAction.linkedObjectName);
                if(newTrackingObject != null) _cameraController.Target = newTrackingObject.transform;
                break;

            case CutsceneActionType.ReturnCameraToPlayer:
                _cameraController.ResetTrackingObject();
                break;

            case CutsceneActionType.SetCameraSize:
                _cameraZoom.SetSize(currentAction.Size);
                break;

            case CutsceneActionType.ResetCameraSize:
                _cameraZoom.ResetSize();
                break;

            // Диалог
            case CutsceneActionType.StartDialogue:
                _dialoguesManager.StartDialogue(currentAction.LinkedDialogue);
                break;

            case CutsceneActionType.BossActionAfterCutscene:
                _bossPhasesManager.BossActionAfterCutscene();
                break;

            case CutsceneActionType.SetPlayerSpawnpoint:
                GameObject someObj = GameObject.Find(currentAction.linkedObjectName);
                if (someObj != null) _livesManager.SetSpawnPoint(someObj.transform);
                break;

            case CutsceneActionType.MoveObject:
                GameObject movingObject = GameObject.Find(currentAction.linkedObjectName);
                GameObject destinationObject = GameObject.Find(currentAction.destinationObjectName);

                if (movingObject != null && destinationObject != null) _objectMover.AnimateMoving(movingObject, destinationObject.transform, ObjectAnimationType.EaseOut, currentAction.animateScale);
                else
                {
                    if (movingObject == null) Debug.LogError($"movingObject: {currentAction.linkedObjectName} не найден");
                    if (destinationObject == null) Debug.LogError($"destinationObject: {currentAction.destinationObjectName} не найден");
                }

                break;

            case CutsceneActionType.SetBossEmotion:
                _bossAnimationController.SetEmotion(currentAction.emotion);
                break;

            case CutsceneActionType.PlaySpecificMusic:
                _gameAudioManager.PlayMusic(currentAction.musicName);
                break;

            default:
                break;
        }

        TryToCallNextAction(currentAction);
    }

    private void TryToCallNextAction(CutsceneAction currentAction)
    {
        // Пытаемся вызвать следующий диалог

        // Сдвигаем указатель
        _currentActionPointer++;

        // Проверяем
        if (currentAction.WaitType == WaitActionType.None) ParseCurrentAction();
        else if (currentAction.WaitType == WaitActionType.WaitForSeconds) StartCoroutine(StartActionWithDelay(currentAction.TimeInSeconds));
        else // WaitActionType.WaitForEndOfTheAction
        {
            // Если текущее действие - НЕ вызов диалога, то мы не можем дождатсья конца действия
            if(currentAction.CAT != CutsceneActionType.StartDialogue)
            {
                Debug.LogError("Братан, WaitForEndOfTheAction используется только когда StartDialogue. " +
                    "Не, я конечно без проблем всё сделаю - базару ноль. " +
                    "Но ты знай, хорошо?");
                ParseCurrentAction();
                return;
            }

            _isSubscribedToDialoguesManager = true;
            _dialoguesManager.OnDialogueEnded += CheckDialogue;
        }
    }

    private IEnumerator StartActionWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ParseCurrentAction();
    }

    private void CheckDialogue(DialogueSO _)
    {
        // Отписываемся
        _isSubscribedToDialoguesManager = false;
        _dialoguesManager.OnDialogueEnded -= CheckDialogue;

        ParseCurrentAction();
    }

    private void OnDestroy()
    {
        if (_isSubscribedToDialoguesManager)
        {
            _isSubscribedToDialoguesManager = false;
            _dialoguesManager.OnDialogueEnded -= CheckDialogue;
        }
    }
}
