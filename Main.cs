
using UnityModManagerNet;
using System.Reflection;

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
            if (value)
            {
                /// TODO: reload settings from local
            }
            enabled = value;
            modEntry.Logger.Log("enabled = " + enabled);
            // Always returns true to allow toggling
            return true;
        }
    }
}
