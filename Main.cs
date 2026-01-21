
using I18N.DotNet;
using PIStatsOverlay.Objects;
using PIStatsOverlay.Patches;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

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
        public class SidebarStatsSettings
        {
            [Draw("Infectiousness", DrawType.Toggle)] public bool ShowInfectiousness = true;
            [Draw("Severity", DrawType.Toggle)] public bool ShowSeverity = true;
            [Draw("Lethality", DrawType.Toggle)] public bool ShowLethality = true;
            [Draw("Air", DrawType.Toggle)] public bool ShowAirTransmission = true;
            [Draw("Sea", DrawType.Toggle)] public bool ShowSeaTransmission = true;
            [Draw("Land", DrawType.Toggle)] public bool ShowLandTransmission = true;
            [Draw("Wealthy", DrawType.Toggle)] public bool ShowWealthy = true;
            [Draw("Poverty", DrawType.Toggle)] public bool ShowPoverty = true;
            [Draw("Urban", DrawType.Toggle)] public bool ShowUrban = true;
            [Draw("Rural", DrawType.Toggle)] public bool ShowRural = true;
            [Draw("Hot", DrawType.Toggle)] public bool ShowHot = true;
            [Draw("Cold", DrawType.Toggle)] public bool ShowCold = true;
            [Draw("Arid", DrawType.Toggle)] public bool ShowArid = true;
            [Draw("Humid", DrawType.Toggle)] public bool ShowHumid = true;
            [Draw("Cure requirement", DrawType.Toggle)] public bool ShowCureRequirement = true;
            [Draw("Effective cure research", DrawType.Toggle)] public bool ShowEffectiveCureResearch = true;
            [Draw("Cure days remaining", DrawType.Toggle)] public bool ShowCureDaysRemaining = true;
            // mutation counter + mutation trigger
            [Draw("Mutation progress", DrawType.Toggle)] public bool ShowMutationProgress = true;
            [Draw("Score", DrawType.Toggle)] public bool ShowScore = true;
        }

        [Draw(DrawType.PopupList)] public Languages Language = Languages.English;
        [Draw("Show sidebar statistics", DrawType.Toggle)] public bool ShowSidebarStats = true;
        [Header("Sidebar Statistics"), Space(5)]
        [Draw("", VisibleOn = "ShowSidebarStats|true")] public SidebarStatsSettings SidebarStats = new SidebarStatsSettings();

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
        public static long gameScore = -1;  // await filled by method
        public static int gameDays = -1;    // await filled by method

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
                overlayObj?.SetActive(false);
            }
            modEntry.Logger.Log("enabled = " + enabled);
            // Always returns true to allow toggling
            return true;
        }
    }
}
