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
                StatFormat.ToString(null, "INF", Main.diseaseStats.infectiousness, "\n"),
                StatFormat.ToString(null, "SEV", Main.diseaseStats.severity, "\n"),
                StatFormat.ToString(null, "LET", Main.diseaseStats.lethality, "\n"),
                StatFormat.ToString(null, "CureNeed", Main.diseaseStats.cureRequirement, "\n", "+0.00E+0;-0.00E+0;0"),
                StatFormat.ToString(null, "CureSpeed", Main.diseaseStats.globalEffectiveCureResearchThisTurn, "\n", "+0.00E+0;-0.00E+0;0"),
                StatFormat.ToString(null, "CureDays", Main.diseaseStats.cureDaysRemaining, "\n", string.Empty),
                StatFormat.ToString(null, "MutCnt", Main.diseaseStats.mutationCounter, "\n"),
                StatFormat.ToString(null, "MutTrig", Main.diseaseStats.mutationTrigger, "\n")
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
            GUI.Label(new Rect(10, 50, 300, 100), GetStatsString(), textStyle);
        }
    }
}
