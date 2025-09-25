using System;
using System.Collections.Generic;
using UnityEditor;

public static class GlobalFlags
{
    private static HashSet<string> flags = new HashSet<string>();
    public static Action<string, bool> onFlagChanged;

    public static void SetFlag(string flagName)
    {
        if (flags.Contains(flagName)) return;
        flags.Add(flagName);
        onFlagChanged?.Invoke(flagName, true);
    }

    public static bool GetFlag(string flagName)
    {
        return flags.Contains(flagName);
    }
    public static void ClearFlag(string flagName)
    {
        if (!flags.Contains(flagName)) return;
        flags.Remove(flagName);
        onFlagChanged?.Invoke(flagName, false);
    }

    public static void ToggleFlag(string flagName)
    {
        if (GetFlag(flagName)) ClearFlag(flagName);
        else SetFlag(flagName);
    }

    public static void ResetAllFlags()
    {
        flags.Clear();
    }

    public static class Flags
    {
        public const string PLAYER_CAN_MOVE = "player_can_move";
        public const string GAME_STARTED = "game_started";
        public const string LEVEL_COMPLETED = "level_completed";
        public const string INPUT_LOCKED = "input_locked";
        public const string GAME_OVER = "game_over";
        public const string GAME_WIN = "game_win";

        // Runner specific

        public const string RUNNER_STAGE_1_PASSED = "runner_stage_1_passed";
        public const string RUNNER_STAGE_2_PASSED = "runner_stage_2_passed";
        public const string RUNNER_STAGE_3_PASSED = "runner_stage_3_passed";
        public const string RUNNER_IS_ROTATING = "runner_is_rotating";
        public const string BLOCK_PLAYER_MOVING = "block_player_moving";
        public const string CAR_TURNING = "car_turning";

        // ShootemUp
        public const string SHOOTEMUP_WAVE_ENDED = "shootemup_wave_ended";
        public const string SHOOTEMUP_START_WAVE = "shootemup_start_wave";
        public const string SHOOTEMUP_ENEMY_DIED = "shootemup_enemy_died";
    }
}