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

        private class TechData
        {
            public Technology Technology;
            public string OriginalDescription;
        }

        /// <summary>
        /// tech id -> TechData
        /// </summary>
        private static readonly Dictionary<string, TechData> modifiedIds = new Dictionary<string, TechData>();

        /// <summary>
        /// An exposed method to reset modified IDs for reloading purposes.
        /// </summary>
        internal static void ResetModifiedIds()
        {
            foreach (var data in modifiedIds.Values)
            {
                if (data.Technology != null)
                {
                    data.Technology.description = data.OriginalDescription;
                }
            }
            modifiedIds.Clear();
            Logger.Log("modifiedIds reseted", SCOPE);
        }

        [HarmonyPatch(nameof(SPDisease.GetEvolveCost))]
        [HarmonyPostfix]
        static void TechDescModify(Technology technology)
        {
            if (!Main.enabled) return;

            try
            {
                if (!modifiedIds.ContainsKey(technology.id))
                {
                    modifiedIds.Add(technology.id, new TechData
                    {
                        Technology = technology,
                        OriginalDescription = technology.description
                    });
                    technology.description = RichString.Format(
                        technology.description,
                        "\n",
                        // Three basic stats
                        StatFormat.ToStringUnlessZero("fd27e6", Main.localizer.Localize("INF"), technology.changeToInfectiousness),
                        StatFormat.ToStringUnlessZero("fdfd57", Main.localizer.Localize("SEV"), technology.changeToSeverity),
                        StatFormat.ToStringUnlessZero("9b2bd7", Main.localizer.Localize("LET"), technology.changeToLethality),
                        // Transmission stats
                        // Transportation
                        StatFormat.ToStringUnlessZero("00ffff", Main.localizer.Localize("Air"), technology.changeToAirTransmission),
                        StatFormat.ToStringUnlessZero("00ffff", Main.localizer.Localize("Sea"), technology.changeToSeaTransmission),
                        StatFormat.ToStringUnlessZero("00ffff", Main.localizer.Localize("Land"), technology.changeToLandTransmission),
                        // Country
                        StatFormat.ToStringUnlessZero("cc6633", Main.localizer.Localize("Wealthy"), technology.changeToWealthy),
                        StatFormat.ToStringUnlessZero("cc6633", Main.localizer.Localize("Poor"), technology.changeToPoverty),
                        StatFormat.ToStringUnlessZero("cc6633", Main.localizer.Localize("Urban"), technology.changeToUrban),
                        StatFormat.ToStringUnlessZero("cc6633", Main.localizer.Localize("Rural"), technology.changeToRural),
                        StatFormat.ToStringUnlessZero("cc6633", Main.localizer.Localize("Hot"), technology.changeToHot),
                        StatFormat.ToStringUnlessZero("cc6633", Main.localizer.Localize("Cold"), technology.changeToCold),
                        StatFormat.ToStringUnlessZero("cc6633", Main.localizer.Localize("Arid"), technology.changeToArid),
                        StatFormat.ToStringUnlessZero("cc6633", Main.localizer.Localize("Humid"), technology.changeToHumid),
                        // Others
                        StatFormat.ToStringUnlessZero("800000", Main.localizer.Localize("Corpse"), technology.changeToCorpseTransmission),
                        // Cure stats
                        StatFormat.ToStringUnlessZero("3366ff", Main.localizer.Localize("CureNeedMult"), technology.changeToCureBaseMultiplier),
                        StatFormat.ToStringUnlessZero("3366ff", Main.localizer.Localize("CureSpdMult"), -technology.changeToResearchInefficiencyMultiplier),
                        // Other stats
                        StatFormat.ToStringUnlessZero(null, Main.localizer.Localize("Mutation"), technology.changeToMutation)
                    );
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
            // Here we do not need to use reset method because SPDisease.Initialise
            // will automatically reset all tech descriptions.
            modifiedIds.Clear();
        }
    }
}
