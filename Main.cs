
using UnityModManagerNet;
using UnityEngine;
using System.Reflection;
using PIStatsOverlay.Objects;
using PIStatsOverlay.Patches;
using I18N.DotNet;

namespace PIStatsOverlay
{
    //
    // Mod settings
    //
    public enum Languages
    {
        English,
        Simplified_Chinese,
    }

    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw(DrawType.PopupList)] public Languages Language = Languages.English;

        public void Apply()
        {
            // Reload translation
            Utils.Logger.Log($"Selected language = {Language}", "Settings.Apply");
            TechDescPatch.ResetModifiedIds();
            switch (Language)
            {
                case Languages.English:
                    Main.localizer.Load("en");
                    break;
                case Languages.Simplified_Chinese:
                    Main.localizer.Load("zh-Hans");
                    break;
            }
        }

        public void OnChange()
        {
            Apply();
        }
    }

    //
    // Entrypoint
    //
    static class Main
    {
        /// <summary>
        /// A global member indicating if the mod is enabled or not. Other methods
        /// should refer to this to en-/dis-able features.
        /// </summary>
        public static bool enabled = true;
        public static UnityModManager.ModEntry modEntry;
        public static Settings settings;
        // Must load resource explicitly
        public static AutoLoadLocalizer localizer { get; } = new AutoLoadLocalizer(
            "Resources.I18N.xml",
            Assembly.GetExecutingAssembly()
        );
        // Static members for other methods
        private static GameObject overlayObj;
        public static DiseaseStats diseaseStats = new DiseaseStats();

        static bool Load(UnityModManager.ModEntry entry)
        {
            // Bind mod entry
            // Should be put at the top
            modEntry = entry;

            // Load settings
            settings = UnityModManager.ModSettings.Load<Settings>(entry);
            settings.Apply();

            // Bind event methods to internal members
            entry.OnToggle = OnToggle;
            entry.OnGUI = OnGUI;
            entry.OnSaveGUI = OnSaveGUI;

            // Bind harmony patches
            new HarmonyLib.Harmony(entry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry entry)
        {
            settings.Draw(entry);
        }

        static void OnSaveGUI(UnityModManager.ModEntry entry)
        {
            UnityModManager.ModSettings.Save(settings, entry);
        }

        static bool OnToggle(UnityModManager.ModEntry entry, bool value)
        {
            enabled = value;
            if (enabled)
            {
                // Reload settings
                settings = UnityModManager.ModSettings.Load<Settings>(entry);
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
