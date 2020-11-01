using System;
using System.Collections.Generic;
using System.Globalization;

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
            int first = text.IndexOf('"');
            int last = text.LastIndexOf('"');
            bool doesContainExplicitSearch = first >= 0 && last > first;
            if (doesContainExplicitSearch) {
                int length = last - first;
                string exp = text.Substring(first + 1, length - 1);
                text.Replace($"\"{exp}\"", " ");
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
