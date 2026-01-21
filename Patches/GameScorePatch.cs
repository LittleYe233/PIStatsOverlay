using HarmonyLib;

namespace PIStatsOverlay.Patches
{
    [HarmonyPatch(typeof(SPDisease), nameof(SPDisease.GameUpdate))]
    public static class GameScorePatch
    {
        static void Postfix(SPDisease __instance)
        {
            if (!Main.enabled) return;
            // It passes when CGameManager.game is StandardGame
            if (CGameManager.game as StandardGame != null)
            {
                bool isScenario;
                switch (CGameManager.gameType)
                {
                    case IGame.GameType.Classic:
                        isScenario = false;
                        break;
                    case IGame.GameType.SpeedRun:
                        Main.gameScore = Main.gameDays;
                        return;
                    case IGame.GameType.Official:
                    case IGame.GameType.Custom:
                        isScenario = true;
                        break;
                    default:
                        return;
                }
                Main.gameScore = __instance.GetScore(won: true, scenario: isScenario);
            }
        }
    }
}
