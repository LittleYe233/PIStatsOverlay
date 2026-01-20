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
            if (CGameManager.game != null && CGameManager.game.CurrentGameState == IGame.GameState.InProgress)
            {
                // Base font size is 20 at 1080p
                float scale = Screen.height / 1080f;
                textStyle.fontSize = Mathf.Max(10, Mathf.RoundToInt(20 * scale));
                DrawStats(scale);
            }
        }

        private string GetStatsString()
        {
            return RichString.Format(
                StatFormat.ToString(null, Main.localizer.Localize("INF"), Main.diseaseStats.infectiousness, "\n"),
                StatFormat.ToString(null, Main.localizer.Localize("SEV"), Main.diseaseStats.severity, "\n"),
                StatFormat.ToString(null, Main.localizer.Localize("LET"), Main.diseaseStats.lethality, "\n"),
                StatFormat.ToString(null, Main.localizer.Localize("CureNeed"), Main.diseaseStats.cureRequirement, "\n", "+0.00E+0;-0.00E+0;0"),
                StatFormat.ToString(null, Main.localizer.Localize("CureSpd"), Main.diseaseStats.globalEffectiveCureResearchThisTurn, "\n", "+0.00E+0;-0.00E+0;0"),
                StatFormat.ToString(null, Main.localizer.Localize("CureDays"), Main.diseaseStats.cureDaysRemaining, "\n", string.Empty),
                StatFormat.ToString(null, Main.localizer.Localize("MutCnt"), Main.diseaseStats.mutationCounter, "\n"),
                StatFormat.ToString(null, Main.localizer.Localize("MutTrig"), Main.diseaseStats.mutationTrigger, "\n")
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
            float rightMargin = 10f * scale;
            float width = 190f * scale;
            float height = 100f * scale;
            float x = Screen.width - rightMargin - width;
            float y = topMargin;
            GUI.Label(new Rect(x, y, width, height), GetStatsString(), textStyle);
        }
    }
}
