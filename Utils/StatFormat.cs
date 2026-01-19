namespace PIStatsOverlay.Utils
{
    internal class StatFormat
    {
        /// <summary>
        /// Convert a stat to string with colored label.
        /// </summary>
        /// <param name="colorHex">Nullable</param>
        /// <param name="statName"></param>
        /// <param name="statValue"></param>
        /// <param name="trailing"></param>
        /// <returns></returns>
        public static string ToString(string colorHex, string statName, float statValue, string trailing = " ", string format = "+0.00;-0.00;0")
        {
            return RichString.Format(
                new RichStringPart.NGUI(colorHex, statName), ": ",
                statValue.ToString(format),
                trailing
            );
        }

        /// <summary>
        /// Convert a stat to string with colored label, unless the value is zero.
        /// </summary>
        /// <param name="colorHex">Nullable</param>
        /// <param name="statName"></param>
        /// <param name="statValue"></param>
        /// <param name="trailing"></param>
        /// <returns></returns>
        public static string ToStringUnlessZero(string colorHex, string statName, float statValue, string trailing = " ")
        {
            if (statValue == 0f)
                return string.Empty;
            return ToString(colorHex, statName, statValue, trailing);
        }
    }
}
