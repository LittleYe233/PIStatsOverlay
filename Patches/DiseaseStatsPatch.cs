using HarmonyLib;
using PIStatsOverlay.Utils;
using System;

namespace PIStatsOverlay.Patches
{
    public class DiseaseStats
    {
        public float infectiousness;
        public float severity;
        public float lethality;
        public float cureRequirement;
        public float researchInefficiencyMultiplier;
        public float globalCureResearchThisTurn;
        public float cureDaysRemaining;
        public float mutationCounter;
        public float mutationTrigger;
        public float globalEffectiveCureResearchThisTurn
        {
            get => globalCureResearchThisTurn * (1f - researchInefficiencyMultiplier);
        }
    }

    [HarmonyPatch(typeof(SPDisease), nameof(SPDisease.GameUpdate))]
    public static class DiseaseStatsPatch
    {
        static void Postfix(SPDisease __instance)
        {
            if (!Main.enabled) return;
            Main.diseaseStats.infectiousness = __instance.globalInfectiousness;
            Main.diseaseStats.severity = __instance.globalSeverity;
            Main.diseaseStats.lethality = __instance.globalLethality;
            Main.diseaseStats.cureRequirement = __instance.cureRequirements;
            Main.diseaseStats.researchInefficiencyMultiplier = __instance.researchInefficiencyMultiplier;
            Main.diseaseStats.globalCureResearchThisTurn = __instance.globalCureResearchThisTurn;
            Main.diseaseStats.cureDaysRemaining = __instance.CureDaysRemaining;
            Main.diseaseStats.mutationCounter = __instance.mutationCounter;
            Main.diseaseStats.mutationTrigger = __instance.mutationTrigger;
        }
    }
}
