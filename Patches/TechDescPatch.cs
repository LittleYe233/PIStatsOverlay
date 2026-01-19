using HarmonyLib;
using PIStatsOverlay.Utils;
using System;
using System.Collections.Generic;

namespace PIStatsOverlay.Patches
{
    [HarmonyPatch(typeof(SPDisease))]
    public static class TechDescPatch
    {
        private static readonly string SCOPE = "TechDescPatch";
        private static readonly HashSet<string> modifiedIds = new HashSet<string>();

        [HarmonyPatch(nameof(SPDisease.GetEvolveCost))]
        [HarmonyPostfix]
        static void TechDescModify(Technology technology)
        {
            if (!Main.enabled) return;

            try
            {
                if (!modifiedIds.Contains(technology.id))
                {
                    technology.description = RichString.Format(
                        technology.description,
                        "\n",
                        // Three basic stats
                        StatFormat.ToStringUnlessZero("fd27e6", "INF", technology.changeToInfectiousness),
                        StatFormat.ToStringUnlessZero("fdfd57", "SEV", technology.changeToSeverity),
                        StatFormat.ToStringUnlessZero("9b2bd7", "LET", technology.changeToLethality),
                        // Transmission stats
                        // Transportation
                        StatFormat.ToStringUnlessZero("00ffff", "Air", technology.changeToAirTransmission),
                        StatFormat.ToStringUnlessZero("00ffff", "Sea", technology.changeToSeaTransmission),
                        StatFormat.ToStringUnlessZero("00ffff", "Land", technology.changeToLandTransmission),
                        // Country
                        StatFormat.ToStringUnlessZero("cc6633", "Wealthy", technology.changeToWealthy),
                        StatFormat.ToStringUnlessZero("cc6633", "Poor", technology.changeToPoverty),
                        StatFormat.ToStringUnlessZero("cc6633", "Urban", technology.changeToUrban),
                        StatFormat.ToStringUnlessZero("cc6633", "Rural", technology.changeToRural),
                        StatFormat.ToStringUnlessZero("cc6633", "Hot", technology.changeToHot),
                        StatFormat.ToStringUnlessZero("cc6633", "Cold", technology.changeToCold),
                        StatFormat.ToStringUnlessZero("cc6633", "Arid", technology.changeToArid),
                        StatFormat.ToStringUnlessZero("cc6633", "Humid", technology.changeToHumid),
                        // Others
                        StatFormat.ToStringUnlessZero("800000", "Corpse", technology.changeToCorpseTransmission),
                        // Cure stats
                        StatFormat.ToStringUnlessZero("3366ff", "CureNeed", technology.changeToCureBaseMultiplier),
                        StatFormat.ToStringUnlessZero("3366ff", "CureSpeed", -technology.changeToResearchInefficiencyMultiplier),
                        // Other stats
                        StatFormat.ToStringUnlessZero(null, "Mutation", technology.changeToMutation)
                    );
                    modifiedIds.Add(technology.id);
                    Logger.Log($"Modified description for Tech ID: {technology.id}", SCOPE);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString(), SCOPE);
            }
        }

        /// <summary>
        /// Initialize necessary variables to make sure patches work correctly.
        /// </summary>
        [HarmonyPatch(nameof(SPDisease.Initialise))]
        [HarmonyPrefix]
        static void PatchInit()
        {
            modifiedIds.Clear();
        }
    }
}
