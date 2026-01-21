using PIStatsOverlay.Utils;
using UnityEngine;

namespace PIStatsOverlay.Objects
{
    public class StatsOverlay : MonoBehaviour
    {
        private GUIStyle textStyle;

        private void Start()
        {
            textStyle = new GUIStyle();
            textStyle.richText = true;
            textStyle.normal.textColor = Color.white;
        }

        private void OnGUI()
        {
            /// TODO: Here the drawing condition should be enhanced to make sure
            /// it is **only** shown when the game is in world map.
            if (CGameManager.game != null && CGameManager.game.CurrentGameState == IGame.GameState.InProgress)
            {
                // Base font size is 18 at 1080p
                float scale = Screen.height / 1080f;
                textStyle.fontSize = Mathf.Max(10, Mathf.RoundToInt(18 * scale));
                DrawStats(scale);
            }
        }

        private string GetStatsString()
        {
            if (!Main.settings.ShowSidebarStats) return string.Empty;
            return RichString.Format(
                Main.settings.SidebarStats.ShowInfectiousness ? StatFormat.ToString(null, Main.localizer.Localize("INF"), Main.diseaseStats.infectiousness, "\n") : "",
                Main.settings.SidebarStats.ShowSeverity ? StatFormat.ToString(null, Main.localizer.Localize("SEV"), Main.diseaseStats.severity, "\n") : "",
                Main.settings.SidebarStats.ShowLethality ? StatFormat.ToString(null, Main.localizer.Localize("LET"), Main.diseaseStats.lethality, "\n") : "",
                Main.settings.SidebarStats.ShowAirTransmission ? StatFormat.ToString(null, Main.localizer.Localize("Air"), Main.diseaseStats.airTransmission, "\n", "+0.0000;-0.0000;0") : "",
                Main.settings.SidebarStats.ShowSeaTransmission ? StatFormat.ToString(null, Main.localizer.Localize("Sea"), Main.diseaseStats.seaTransmission, "\n", "+0.0000;-0.0000;0") : "",
                Main.settings.SidebarStats.ShowLandTransmission ? StatFormat.ToString(null, Main.localizer.Localize("Land"), Main.diseaseStats.landTransmission, "\n", "+0.0000;-0.0000;0") : "",
                Main.settings.SidebarStats.ShowWealthy ? StatFormat.ToString(null, Main.localizer.Localize("Wealthy"), Main.diseaseStats.wealthy, "\n", "+0.0000;-0.0000;0") : "",
                Main.settings.SidebarStats.ShowPoverty ? StatFormat.ToString(null, Main.localizer.Localize("Poor"), Main.diseaseStats.poverty, "\n", "+0.0000;-0.0000;0") : "",
                Main.settings.SidebarStats.ShowUrban ? StatFormat.ToString(null, Main.localizer.Localize("Urban"), Main.diseaseStats.urban, "\n", "+0.0000;-0.0000;0") : "",
                Main.settings.SidebarStats.ShowRural ? StatFormat.ToString(null, Main.localizer.Localize("Rural"), Main.diseaseStats.rural, "\n", "+0.0000;-0.0000;0") : "",
                Main.settings.SidebarStats.ShowHot ? StatFormat.ToString(null, Main.localizer.Localize("Hot"), Main.diseaseStats.hot, "\n", "+0.0000;-0.0000;0") : "",
                Main.settings.SidebarStats.ShowCold ? StatFormat.ToString(null, Main.localizer.Localize("Cold"), Main.diseaseStats.cold, "\n", "+0.0000;-0.0000;0") : "",
                Main.settings.SidebarStats.ShowArid ? StatFormat.ToString(null, Main.localizer.Localize("Arid"), Main.diseaseStats.arid, "\n", "+0.0000;-0.0000;0") : "",
                Main.settings.SidebarStats.ShowHumid ? StatFormat.ToString(null, Main.localizer.Localize("Humid"), Main.diseaseStats.humid, "\n", "+0.0000;-0.0000;0") : "",
                Main.settings.SidebarStats.ShowCureRequirement ? StatFormat.ToString(null, Main.localizer.Localize("CureNeed"), Main.diseaseStats.cureRequirement, "\n", "+0.000E+0;-0.000E+0;0") : "",
                Main.settings.SidebarStats.ShowEffectiveCureResearch ? StatFormat.ToString(null, Main.localizer.Localize("CureSpd"), Main.diseaseStats.globalEffectiveCureResearchThisTurn, "\n", "+0.000E+0;-0.000E+0;0") : "",
                Main.settings.SidebarStats.ShowCureDaysRemaining ? StatFormat.ToString(null, Main.localizer.Localize("CureDays"), Main.diseaseStats.cureDaysRemaining, "\n", string.Empty) : "",
                Main.settings.SidebarStats.ShowMutationProgress ? StatFormat.ToString(null, Main.localizer.Localize("MutCnt"), Main.diseaseStats.mutationCounter, "\n") : "",
                Main.settings.SidebarStats.ShowMutationProgress ? StatFormat.ToString(null, Main.localizer.Localize("MutTrig"), Main.diseaseStats.mutationTrigger, "\n") : "",
                Main.settings.SidebarStats.ShowScore ? StatFormat.ToString(null, Main.localizer.Localize("Score"), Main.gameScore, "\n", string.Empty) : ""
            );
        }

        private void DrawStats(float scale)
        {
            //////////////////////////////////////////////
            //
            // *--------------------------------------*
            // |                     ^                |
            // |                top margin            |
            // |                     v                |
            // |             *-------------*          |
            // |             |    label    |< right  >|
            // |             |     rect    |  margin  |
            // |             *-------------*          |
            // |                                      |
            // *--------------------------------------*
            //
            //////////////////////////////////////////////

            float topMargin = .3f * Screen.height;
            float rightMargin = 9f * scale;
            float width = 189f * scale;
            float height = 100f * scale;
            float x = Screen.width - rightMargin - width;
            float y = topMargin;
            GUI.Label(new Rect(x, y, width, height), GetStatsString(), textStyle);
        }
    }
}
