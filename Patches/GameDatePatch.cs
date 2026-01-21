using HarmonyLib;
using System;

namespace PIStatsOverlay.Patches
{
    [HarmonyPatch(typeof(CHUDScreen), nameof(CHUDScreen.SetDay))]
    public static class GameDatePatch
    {
        static void Postfix(ref DateTime ___startDate)
        {
            if (!Main.enabled) return;
            // I do not know why here should be +1
            Main.gameDays = (CGameManager.currentGameDate - ___startDate).Days + 1;
        }
    }
}
