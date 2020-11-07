using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Diary.Logic {
    public static class Extensions {
        /// <summary>
        /// A Contains() Method extension of string.
        /// Will ignore cases unlike original method.
        /// </summary>
        /// <param name="str">the original text</param>
        /// <param name="toCheck">what to find inside</param>
        /// <returns>if requested substring was found in the original string</returns>
        public static bool ContainsInsensitive(this string str, string toCheck) {
            str = str.ToLower();
            toCheck = toCheck.ToLower();
            return str.Contains(toCheck);
        }

        /// <summary>
        /// Removes all whitespace characters from text
        /// In this app it used to compare entree bodies to avoid duplicates with different spacing
        /// </summary>
        /// <param name="str"></param>
        /// <returns>String without any whitespaces</returns>
        public static string RemoveWhiteSpace(this string str) {
            return new string(str.ToCharArray()
        .Where(c => !char.IsWhiteSpace(c))
        .ToArray());
        }

        /// <summary>
        /// Works essentially like [start:end] in Python
        /// A more convenient way of using Substring (No need to calculate the length you need)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fromStart">Distance from start</param>
        /// <param name="fromEnd">Distance from end</param>
        /// <returns>Subset of string</returns>
        public static string Subset(this string str, int fromStart, int fromEnd) {
            return str.Substring(fromStart, str.Length - fromEnd - fromStart);
        }

        /// <summary>
        /// Checks if a string is bordered by 2 strings
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startBorder">First border</param>
        /// <param name="endBorder">Last border</param>
        /// <returns>bool</returns>
        public static bool HasBorders(this string str, string startBorder, string endBorder) {
            return str.StartsWith(startBorder) && str.EndsWith(endBorder);
        }

        /// <summary>
        /// Used to query search selected text.
        /// The query is split up to words and the function
        /// checks if every single of those words is contained within the text.
        /// </summary>
        /// <param name="str">original text</param>
        /// <param name="words">query split into array of words</param>
        /// <param name="i">current place in words</param>
        /// <returns>if str contains all words</returns>
        public static bool QuerySearch(this string str, string[] words, int i = 0) {
            if (words.Length == 0) {
                return false;
            }
            if (i == words.Length - 1) {
                return str.ContainsInsensitive(words[i]);
            }
            return str.ContainsInsensitive(words[i]) && str.QuerySearch(words, i + 1);
        }

        /// <summary>
        /// Splits text by removing all whitespace and empty entries
        /// </summary>
        /// <param name="text"></param>
        /// <returns>string split into words</returns>
        public static string[] ToWords(this string text) {
            return text.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Returns text split into query parts
        /// If 2 " are entered, whatever between them will be explicit and not be split up
        /// Only one explicit is possible with a query
        /// </summary>
        /// <param name="text">Query</param>
        /// <returns>Array of query parts</returns>
        public static string[] Queryable(this string text) {
            List<string> parts = new List<string>();
            var first = text.IndexOf("\"", StringComparison.Ordinal);
            var last = text.LastIndexOf("\"", StringComparison.Ordinal);
            var doesContainExplicitSearch = first >= 0 && last > first;
            if (doesContainExplicitSearch) {
                var length = last - first;
                var exp = text.Substring(first + 1, length - 1);
                text = text.Replace($"\"{exp}\"", " ");
                parts.Add(exp);
                parts.AddRange(text.ToWords());
            } else {
                parts.AddRange(text.ToWords());
            }
            return parts.ToArray();
        }

        /// <summary>
        /// Extension method of DateTime that returns a complete date
        /// Format: [Long date] & [Long time]
        /// </summary>
        /// <param name="d">DateTime object</param>
        /// <returns>representation of the time as string</returns>
        public static string FullDateAndTime(this DateTime d) {
            return $"{d.ToLongDateString()} at {d.ToLongTimeString()}";
        }

        /// <summary>
        /// Method used to turn any string into Title format
        /// Title format is when every word begins capatilized.
        /// </summary>
        /// <param name="str">any text</param>
        /// <returns>Text In Title Format</returns>
        public static string ToTitle(this string str) {
            return new CultureInfo("en").TextInfo.ToTitleCase(str.ToLower());
        }
    }
}
