using System;
using System.Collections;
using System.Collections.Generic;

namespace Diary.Logic {
    /// <summary>
    /// Every log is to be an error.
    /// Will not log successful operations.
    /// </summary>
    public static class Logger {
        private static List<string> Logged { get; } = new List<string>();
        public static bool HasErrorOccurred => ExceptionNumber > 0;
        private static int ExceptionNumber { get; set; } = 0;

        public static List<string> GetLog() => Logged;

        /// <summary>
        /// Adds custom message to log
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message) {
            ExceptionNumber++;
            Logged.Add(message);
        }

        /// <summary>
        /// Logs the entire Exception format
        /// </summary>
        /// <param name="e"></param>
        public static void Log(Exception e) {
            ExceptionNumber++;
            Logged.Add($"--->\tException {ExceptionNumber}\t<---");
            Logged.Add(e.Message);
            Logged.Add(e.Source);
            Logged.AddRange(e.Data.ToTable());
            Logged.Add($"--->\tEnd of exception\t<---");
        }

        /// <summary>
        /// Displays the inner dictionary of the Exception.Data in a readable format
        /// </summary>
        /// <param name="dict">e.Data</param>
        /// <returns>list of string</returns>
        private static List<string> ToTable(this IDictionary dict) {
            var tbl = new List<string>();
            foreach (DictionaryEntry pair in dict) {
                tbl.Add(string.Format("{0, 0}: {1, 1}", pair.Key, pair.Value));
            }
            return tbl;
        }

        /// <summary>
        /// Exports the log file.
        /// Will only work if errors have indeed occurred.
        /// </summary>
        public static void ExportLog() {
            if (HasErrorOccurred) {
                Native.ListToFile("log.txt", Logged);
            }
        }
    }
}
