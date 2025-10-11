using System;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalFlags
{
    private static readonly HashSet<Flags> _activeFlags = new HashSet<Flags>();

    public static event Action<Flags, bool> onFlagChangedEnum;
    public static event Action<string, bool> onFlagChanged;

    public static void SetFlag(Flags flag)
    {
        if (_activeFlags.Contains(flag)) return;

        _activeFlags.Add(flag);
        onFlagChangedEnum?.Invoke(flag, true);
        onFlagChanged?.Invoke(flag.ToString(), true);
    }

    public static bool GetFlag(Flags flag)
    {
        return _activeFlags.Contains(flag);
    }

    public static void ClearFlag(Flags flag)
    {
        if (!_activeFlags.Contains(flag)) return;

        _activeFlags.Remove(flag);
        onFlagChangedEnum?.Invoke(flag, false);
        onFlagChanged?.Invoke(flag.ToString(), false);
    }

    public static void ToggleFlag(Flags flag)
    {
        if (GetFlag(flag))
            ClearFlag(flag);
        else
            SetFlag(flag);
    }

    public static void ResetAllFlags()
    {
        _activeFlags.Clear();
    }

    public static IReadOnlyCollection<Flags> GetActiveFlags()
    {
        return _activeFlags;
    }
}

public enum Flags
{
    // General
    PlayerCanMove,
    GameStarted,
    LevelCompleted,
    InputLocked,
    GameOver,
    GameWin,

    // Runner specific
    RunnerStage1Passed,
    RunnerStage2Passed,
    RunnerStage3Passed,
    RunnerIsRotating,
    BlockPlayerMoving,
    CarTurning,

    // ShootEmUp
    ShootEmUpWaveEnded,
    ShootEmUpStartWave,
    ShootEmUpEnemyDied
}