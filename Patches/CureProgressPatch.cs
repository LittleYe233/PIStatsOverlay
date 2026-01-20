using HarmonyLib;

namespace PIStatsOverlay.Patches
{
    [HarmonyPatch(typeof(CHUDScreen), nameof(CHUDScreen.SetCurePercent))]
    public static class CureProgressPatch
    {
        /// <summary>
        /// A flag if the width of ___cureText is adjusted or not
        /// </summary>
        private static bool widthAdjusted = false;

        static void Postfix(ref UILabel ___cureText, ref UISlider ___cureBar)
        {
            ___cureText.text = (___cureBar.value * 100f).ToString("0.00") + "%";

            if (!widthAdjusted && ___cureText != null)
            {
                // Magic number, but it works
                ___cureText.width = 120;
                ___cureText.MarkAsChanged();
                widthAdjusted = true;
            }
        }
    }
}
