using HarmonyLib;

namespace PIStatsOverlay.Patches
{
    public class DiseaseStats
    {
        public float infectiousness;
        public float severity;
        public float lethality;
        public float airTransmission;
        public float seaTransmission;
        public float landTransmission;
        public float corpseTransmission;
        public float wealthy;
        public float poverty;
        public float urban;
        public float rural;
        public float hot;
        public float cold;
        public float arid;
        public float humid;
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
            Main.diseaseStats.airTransmission = __instance.airTransmission;
            Main.diseaseStats.seaTransmission = __instance.seaTransmission;
            Main.diseaseStats.landTransmission = __instance.landTransmission;
            Main.diseaseStats.corpseTransmission = __instance.corpseTransmission;
            Main.diseaseStats.wealthy = __instance.wealthy;
            Main.diseaseStats.poverty = __instance.poverty;
            Main.diseaseStats.urban = __instance.urban;
            Main.diseaseStats.rural = __instance.rural;
            Main.diseaseStats.hot = __instance.hot;
            Main.diseaseStats.cold = __instance.cold;
            Main.diseaseStats.arid = __instance.arid;
            Main.diseaseStats.humid = __instance.humid;
            Main.diseaseStats.cureRequirement = __instance.cureRequirements;
            Main.diseaseStats.researchInefficiencyMultiplier = __instance.researchInefficiencyMultiplier;
            Main.diseaseStats.globalCureResearchThisTurn = __instance.globalCureResearchThisTurn;
            Main.diseaseStats.cureDaysRemaining = __instance.CureDaysRemaining;
            Main.diseaseStats.mutationCounter = __instance.mutationCounter;
            Main.diseaseStats.mutationTrigger = __instance.mutationTrigger;
        }
    }
}
