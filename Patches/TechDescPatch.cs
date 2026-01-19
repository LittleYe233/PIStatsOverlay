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

        /// <summary>
        /// Convert a stat to string with colored label, unless the stat value is zero.
        /// </summary>
        /// <param name="colorHex">Nullable</param>
        /// <param name="statName"></param>
        /// <param name="statValue"></param>
        /// <param name="trailingSpace"></param>
        /// <returns></returns>
        private static string StatToStringUnlessZero(string colorHex, string statName, float statValue, bool trailingSpace = true)
        {
            if (statValue == 0f)
                return string.Empty;
            return RichString.Format(
                new RichStringPart.NGUI(colorHex, statName), ": ",
                statValue.ToString("+0.00;-0.00;0"),
                trailingSpace ? " " : string.Empty
            );
        }

        [HarmonyPatch(nameof(SPDisease.GetEvolveCost))]
        [HarmonyPostfix]
        public static void TechDescModify(Technology technology)
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
                        StatToStringUnlessZero("fd27e6", "INF", technology.changeToInfectiousness),
                        StatToStringUnlessZero("fdfd57", "SEV", technology.changeToSeverity),
                        StatToStringUnlessZero("9b2bd7", "LET", technology.changeToLethality),
                        // Transmission stats
                        // Transportation
                        StatToStringUnlessZero("00ffff", "Air", technology.changeToAirTransmission),
                        StatToStringUnlessZero("00ffff", "Sea", technology.changeToSeaTransmission),
                        StatToStringUnlessZero("00ffff", "Land", technology.changeToLandTransmission),
                        // Country
                        StatToStringUnlessZero("cc6633", "Wealthy", technology.changeToWealthy),
                        StatToStringUnlessZero("cc6633", "Poor", technology.changeToPoverty),
                        StatToStringUnlessZero("cc6633", "Urban", technology.changeToUrban),
                        StatToStringUnlessZero("cc6633", "Rural", technology.changeToRural),
                        StatToStringUnlessZero("cc6633", "Hot", technology.changeToHot),
                        StatToStringUnlessZero("cc6633", "Cold", technology.changeToCold),
                        StatToStringUnlessZero("cc6633", "Arid", technology.changeToArid),
                        StatToStringUnlessZero("cc6633", "Humid", technology.changeToHumid),
                        // Others
                        StatToStringUnlessZero("800000", "Corpse", technology.changeToCorpseTransmission),
                        // Cure stats
                        StatToStringUnlessZero("3366ff", "CureNeed", technology.changeToCureBaseMultiplier),
                        StatToStringUnlessZero("3366ff", "CureSpeed", -technology.changeToResearchInefficiencyMultiplier),
                        // Other stats
                        StatToStringUnlessZero(null, "Mutation", technology.changeToMutation)
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
        public static void PatchInit()
        {
            modifiedIds.Clear();
        }
    }
}
