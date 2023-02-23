using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Blaze.Utility.Extend;
using UnityEngine;

namespace Blaze.Utility
{
    /// <summary>
    /// 文字样式
    /// </summary>
    public class FontStyle
    {
        private static string _prefix;
        private static string _suffix;

        private static void ConvertToHtml(string format)
        {
            _prefix = $"<{format}>";
            _suffix = $"</{format}>";
        }

        private static string[] styles = { "b", "i" };

        /// <summary>
        /// 获取正则表达式组的替换文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        public static string GetStyledText(string text, GroupCollection groups)
        {
            string style = string.Empty;

            for (int i = 0; i < groups.Count; i++)
            {
                style = GetPossibleValue(groups[i].Value);
                if (style != string.Empty) break;
            }

            if (styles.Contains(style))
                ConvertToHtml(style);
            else
                return text;

            return _prefix + text + _suffix;
        }

        public static string GetPossibleValue(string value)
        {
            return styles.Contains(value) ? value : string.Empty;
        }
    }

    /// <summary>
    /// 系统表情包
    /// </summary>
    public class Emoji
    {
        /// <summary>
        /// 系统默认的Emoji
        /// </summary>
        private static readonly Dictionary<string, string> Emojis = new Dictionary<string, string>()
        {
            {"love", '\u2764'.ToString()},
            {"what", @"╮(￣▽￣)╭"},
            {"bug", @"ㄟ(▔,▔)ㄏ"},
            {"todo", @"  (●ﾟωﾟ●)"},
            {"fix", @"ヽ(･ω･´ﾒ)"},
        };

        /// <summary>
        /// 获取Emoji
        /// </summary>
        /// <param name="text"></param>
        /// <param name="emoji"></param>
        /// <returns></returns>
        public static string GetEmoji(string text, string emoji)
        {
            emoji = emoji.Trim();
            if (!Emojis.ContainsKey(emoji)) return text;

            string spaces = Regex.Replace(text, "[^ ]", "");
            return spaces + Emojis[emoji];
        }

        /// <summary>
        /// 添加新的Emoji
        /// </summary>
        /// <param name="emoji"></param>
        /// <param name="content"></param>
        public static void AddEmoji(string emoji, string content)
        {
            Emojis[emoji] = content;
        }
    }

    /// <summary>
    /// 字体颜色定义
    /// </summary>
    public class FontColor
    {
        private static readonly Dictionary<string, Color> Colors = new Dictionary<string, Color>()
        {
            // Built-in Colors

            {"red", Color.red},
            {"yellow", Color.yellow},
            {"green", Color.green},
            {"blue", Color.blue},
            {"cyan", Color.cyan},
            {"magenta", Color.magenta},

            // Hex Example

            {"orange", "#FFA500".ToColor()},
            {"olive", "#808000".ToColor()},
            {"purple", "#800080".ToColor()},
            {"darkred", "#8B0000".ToColor()},
            {"darkgreen", "#006400".ToColor()},
            {"darkorange", "#FF8C00".ToColor()},
            {"lightred", "#EB5050".ToColor()},
            {"lightgreen", "#506450".ToColor()},
            {"lightorange", "#FF8C50".ToColor()},
            {"lightblue", "#5050FF".ToColor()},
            {"gold", "#FFD700".ToColor()},
        };

        private static readonly Dictionary<string, Color> RainBowColors = new Dictionary<string, Color>()
        {
            // Built-in Colors

            {"red", Color.red},
            {"orange", "#FFA500".ToColor()},
            {"yellow", Color.yellow},
            {"green", Color.green},
            {"blue", "#0000FF".ToColor()},
            {"violet", "#8B00FF".ToColor()},
        };


        private static string prefix;

        private const string Suffix = "</color>";


        public static string GetColorfulText(string text, GroupCollection groups)
        {
            string colorName = string.Empty;

            for (int i = 0; i < groups.Count; i++)
            {
                colorName = GetPossibleValue(groups[i].Value);

                if (colorName != string.Empty) break;
            }


            if (colorName == "rainbow")
            {
                string message = "";
                int counter = 0;
                foreach (char c in text)
                {
                    if (counter >= RainBowColors.Count) counter = 0;
                    var randomColor = RainBowColors.ElementAt(counter).Key;
                    prefix = ConvertToHtml(RainBowColors[randomColor]);
                    message += prefix + c + Suffix;
                    counter++;
                }

                return message;
            }


            if (Colors.ContainsKey(colorName))
                prefix = ConvertToHtml(Colors[colorName]);
            else
                return text;

            return prefix + text + Suffix;
        }

        // Convert Color to HtmlString
        private static string ConvertToHtml(Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
        }

        public static string GetPossibleValue(string value)
        {
            return Colors.ContainsKey(value) || value == "rainbow" ? value : string.Empty;
        }
    }

    /// <summary>
    ///  Co._(@"I:orange:b; love:red:b; Unity:yellow:b;ColorfullllllllllllllllText:rainbow:b;");
    ///  Co._(@"sniper:orange:b; bug:green:b;");
    /// </summary>
    public class Co
    {
        /// <summary>
        /// 快捷的添加色彩
        /// </summary>
        /// <param name="input"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string _(string input, string format)
        {
            return _(input + format);
        }

        public static string _(string input)
        {
            string pattern = @"([^;:]*)\:?([^;:]*)\:?([^;:]*)\:?([^;:]*)\;";
            RegexOptions options = RegexOptions.Multiline;
            Regex regex = new Regex(pattern, options);
            MatchCollection matches = regex.Matches(input);

            string text = string.Empty;


            for (int i = 0; i < matches.Count; i++)
            {
                var emoji = Emoji.GetEmoji(matches[i].Groups[1].Value, matches[i].Groups[1].Value);
                var colorfulText = FontColor.GetColorfulText(emoji, matches[i].Groups);
                var styledText = FontStyle.GetStyledText(colorfulText, matches[i].Groups);

                text += styledText;
            }

            // Debug.Log(text);
            return text;
        }
    }
}