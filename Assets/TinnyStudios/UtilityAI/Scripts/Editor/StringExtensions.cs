using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

namespace TinnyStudios.AIUtility.Editor
{
    /// <summary>
    /// A set of string extensions. Mostly around Rich Texts.
    /// </summary>
    public static class StringExtensions
    {
        public static string ToRichText(this string text, string tag, string value = "")
        {
            return $"<{tag}{value}>{text}</{tag}>";
        }

        public static string ToBold(this string text)
        {
            return text.ToRichText("b");
        }

        public static string ToColor(this string text, Color color)
        {
            return text.ToRichText("color", $"=#{ColorUtility.ToHtmlStringRGB(color)}");
        }
    }
}