using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIStatsOverlay.Utils
{
    public static class Logger
    {
        public static void Log(string msg, string scope = "")
        {
            if (string.IsNullOrEmpty(scope))
            {
                Main.modEntry.Logger.Log(msg);
            }
            else
            {
                Main.modEntry.Logger.Log($"[{scope}] {msg}");
            }
        }

        public static void Error(string msg, string scope = "")
        {
            if (string.IsNullOrEmpty(scope))
            {
                Main.modEntry.Logger.Error(msg);
            }
            else
            {
                Main.modEntry.Logger.Error($"[{scope}] {msg}");
            }
        }

        public static void Critical(string msg, string scope = "")
        {
            if (string.IsNullOrEmpty(scope))
            {
                Main.modEntry.Logger.Critical(msg);
            }
            else
            {
                Main.modEntry.Logger.Critical($"[{scope}] {msg}");
            }
        }
    }
}
