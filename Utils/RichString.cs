using System;
using System.Text;

namespace PIStatsOverlay.Utils
{
    internal abstract class RichStringPart
    {
        /// <summary>
        /// The real string to be shown.
        /// </summary>
        public abstract string Value { get; }

        /// <summary>
        /// Private constructor prevents external inheritance.
        /// </summary>
        private RichStringPart() { }

        /// <summary>
        /// Auto type conversion from string to `RichStringPart.Normal`.
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator RichStringPart(string input)
        {
            return input == null ? null : new Normal(input);
        }

        /// <summary>
        /// Normal plain strings.
        /// </summary>
        public sealed class Normal : RichStringPart
        {
            public override string Value { get; }
            public Normal(string value) => Value = value;
        }

        /// <summary>
        /// NGUI-format colored strings. `colorHex` can be null, then it behaves
        /// like plain strings.
        /// </summary>
        public sealed class NGUI : RichStringPart
        {
            public override string Value { get; }
            public NGUI(string colorHex, string value) =>
                Value = String.IsNullOrEmpty(colorHex) ? value : $"[{colorHex}]{value}[-]";
        }
    }

    internal static class RichString
    {
        public static string Format(params RichStringPart[] parts)
        {
            StringBuilder sb = new StringBuilder();
            foreach (RichStringPart part in parts)
            {
                if (part != null)
                    sb.Append(part.Value);
            }
            return sb.ToString();
        }
    }
}
