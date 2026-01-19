
using UnityModManagerNet;
using UnityEngine;
using System.Reflection;
using PIStatsOverlay.Objects;
using PIStatsOverlay.Patches;

namespace PIStatsOverlay
{
    static class Main
    {
        /// <summary>
        /// A global member indicating if the mod is enabled or not. Other methods
        /// should refer to this to en-/dis-able features.
        /// </summary>
        public static bool enabled = true;
        public static UnityModManager.ModEntry modEntry;
        // Static members for other methods
        private static GameObject overlayObj;
        public static DiseaseStats diseaseStats = new DiseaseStats();

        static bool Load(UnityModManager.ModEntry entry)
        {
            // Bind mod entry
            modEntry = entry;

            // Bind event methods to internal members
            entry.OnToggle = OnToggle;

            // Bind harmony patches
            new HarmonyLib.Harmony(entry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry entry, bool value)
        {
            enabled = value;
            if (enabled)
            {
                // Activate overlay object
                if (overlayObj == null)
                {
                    overlayObj = new GameObject("PIStatsOverlay_OverlayObject");
                    Object.DontDestroyOnLoad(overlayObj);
                    overlayObj.AddComponent<StatsOverlay>();
                }
                overlayObj.SetActive(true);
            }
            else
            {
                // Deactivate overlay object
                if (overlayObj != null)
                {
                    overlayObj.SetActive(false);
                }
            }
            modEntry.Logger.Log("enabled = " + enabled);
            // Always returns true to allow toggling
            return true;
        }
    }
}
