using System;
using UnityEngine;

public class BossPhasesManager : MonoBehaviour
{
    public static BossPhasesManager Instance { get; private set; }
    private PlatformerCutscenesManager _cutscenesManager;
    [SerializeField] private PlatformerCutscene[] phasesCutscenes;
    private PhaseID _currentPhase = PhaseID.First;
    public PhaseID CurrentPhase => _currentPhase;

    public PhaseID NextPhaseID
    {
        get
        {
            if (CurrentPhase == PhaseID.First) return PhaseID.Second;
            else if (CurrentPhase == PhaseID.Second) return PhaseID.Third;
            else return PhaseID.None;
        }
    }

    public event Action<PhaseID> OnPhaseStarted;
    public event Action<PhaseID> OnPhaseEnded;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _cutscenesManager = PlatformerCutscenesManager.Instance;
    }

    public void StartBossBattle()
    {
        // Начинаем цепочку битвы с боссом
        LaunchPhaseCutscene();
    }

    private void LaunchPhaseCutscene()
    {
        // Начинаем кат-сцену

        // Можно было подвязать это через делегат
        // Но почему бы и нет
        _cutscenesManager.StartCutscene(phasesCutscenes[(int)_currentPhase]); 
    }

    public void BossActionAfterCutscene()
    {
        // Действия, когда кат-сцена фазы кончилась
        OnPhaseStarted?.Invoke(_currentPhase);
    }

    public void TryToEndPhase()
    {
        // Проверка, можно ли закончить фазу
        if (_currentPhase == PhaseID.Third) return; // Не можем закончить если третья фаза или выше - там другой механизм

        OnPhaseEnded?.Invoke(_currentPhase);

        _currentPhase = NextPhaseID;
        LaunchPhaseCutscene();
    }
}

public enum PhaseID
{
    First,
    Second,
    Third,
    None
}
