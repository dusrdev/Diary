using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Threading;

namespace Diary.Logic {
    public static class Native {
        public static void Invoke(Action action) => Dispatcher.CurrentDispatcher.Invoke(action);

        /// <summary>
        /// Function that attempts to remove file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="extension"></param>
        /// <returns>Indication of success</returns>
        public static bool RemoveFile(string fileName, string extension = ".dat") {
            fileName += extension;
            if (!File.Exists(fileName)) {
                return false;
            } else {
                try {
                    File.Delete(fileName);
                } catch (Exception e) {
                    Logger.Log(e);
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Exports a list strings to a .txt file, every string to new line.
        /// </summary>
        /// <param name="filename">filename -> including .txt</param>
        /// <param name="lst">list of strings to export</param>
        public static void ListToFile(string filename, List<string> lst) {
            try {
                using (TextWriter w = new StreamWriter(filename)) {
                    foreach (var line in lst) {
                        w.WriteLine(line);
                    }
                }
            } catch (Exception e) {
                e.Data.Add("DevMessage", "Saving list to file failed.");
                Logger.Log(e);
            }
        }

        /// <summary>
        /// checks of file exists in exe directory
        /// </summary>
        /// <param name="filename">including extension and case sensitive</param>
        /// <returns>result</returns>
        public static bool DoesFileExist(string filename) {
            return File.Exists(filename);
        }
    }
}