using System.Collections.Generic;
using System.Linq;
using static Diary.Logic.Logger;

namespace Diary.Logic {
    //TODO: Check for more typos
    public class ActiveDatabase {
        public List<DiaryEntry> CurrentEntries { get; set; }
        public List<User> Udb { get; set; }
        public User CurrentUser { get; set; }
        public MySerializer EntrySaver { get; set; }
        public MySerializer UserSaver { get; set; }

        public ActiveDatabase() {
            UserSaver = new MySerializer("Credentials");
            if (Native.DoesFileExist("Udb.dat")) {
                Udb = UserSaver.DeserializeFromFile<List<User>>("Udb");
            } else {
                Udb = new List<User>();
            }
        }

        /// <summary>
        /// Creates a new user from the login window
        /// </summary>
        /// <param name="newUser">Credentials</param>
        /// <returns>indication of success</returns>
        public bool AddUser(User newUser) {
            if (Udb.Exists(u => u.Username == newUser.Username)) {
                Log("Tried to add existing user");
                return false;
            }
            newUser.UserID = Udb.Count > 0 ? Udb.Last().UserID + 1 : 0;
            Udb.Add(newUser);
            UserSaver.SerializeToFile(Udb, "Udb");
            CurrentUser = newUser;
            EntrySaver = new MySerializer(CurrentUser.Password);
            return true;
        }

        /// <summary>
        /// Search entries by query
        /// </summary>
        /// <param name="query">search query</param>
        /// <returns>Results (Not found if empty)</returns>
        public List<DiaryEntry> Search(string query) {
            var results = new List<DiaryEntry>();
            if (CurrentEntries == null || CurrentEntries.Count == 0) {
                return results;
            }
            if (query == "*") {
                return CurrentEntries;
            }
            var sQuery = query.Queryable();
            if (query.HasBorders("[", "]")) {
                sQuery = query.Subset(1, 1).Queryable();
                return CurrentEntries.FindAll(e => e.Date.QuerySearch(sQuery));
            }
            foreach (var entry in CurrentEntries) {
                if (entry.Title.QuerySearch(sQuery) || entry.Body.QuerySearch(sQuery)) {
                    results.Add(entry);
                }
            }
            return results;
        }

        /// <summary>
        /// Remove entry by id (displayed in search window)
        /// </summary>
        /// <param name="queryId">ID of desired entry</param>
        /// <returns>indication of success</returns>
        public bool RemoveEntry(int queryId) {
            if (!CurrentEntries.Exists(e => e.ID == queryId)) {
                return false;
            }
            CurrentEntries.RemoveAll(e => e.ID == queryId);
            for (var i = 0; i < CurrentEntries.Count; i++) {
                CurrentEntries[i].ID = i;
            }
            EntrySaver.SerializeToFile(CurrentEntries, $"Edb{CurrentUser.UserID}");
            return true;
        }

        /// <summary>
        /// Add entry
        /// </summary>
        /// <param name="title">Entry title</param>
        /// <param name="body">Entry body</param>
        /// <returns>indication of success</returns>
        public bool AddEntry(string title, string body) {
            CurrentEntries ??= new List<DiaryEntry>();
            var cleanBody = body.RemoveWhiteSpace();
            if (CurrentEntries.Exists(e => e.Body.RemoveWhiteSpace() == cleanBody)) {
                return false;
            }
            var newEntryId = CurrentEntries.Count > 0 ? CurrentEntries.Last().ID + 1 : 0;
            var newEntry = new DiaryEntry(title, body, newEntryId);
            CurrentEntries.Add(newEntry);
            EntrySaver.SerializeToFile(CurrentEntries, $"Edb{CurrentUser.UserID}");
            return true;
        }

        /// <summary>
        /// Removes current user!
        /// The app will remain open but the user will not be able to re-login.
        /// </summary>
        /// <returns>Indication of success</returns>
        public bool RemoveCurrentUser() {
            if (CurrentUser == null) {
                return false;
            }
            Udb.RemoveAll(u => u.UserID == CurrentUser.UserID);
            UserSaver.SerializeToFile(Udb, "Udb");
            Native.RemoveFile($"Edb{CurrentUser.UserID}");
            return true;
        }

        public bool Login(string username, string password) {
            if (!Udb.Exists(u => u.Username == username)) {
                return false;
            }
            var matchingUser = Udb.Find(u => u.Username == username);
            if (!Aes.IsValid(password, matchingUser.HashedPassword)) return false;
            CurrentUser = matchingUser;
            CurrentUser.Password = password;
            EntrySaver = new MySerializer(CurrentUser.Password);
            if (Native.DoesFileExist($"Edb{CurrentUser.UserID}.dat")) {
                CurrentEntries = EntrySaver.DeserializeFromFile<List<DiaryEntry>>($"Edb{CurrentUser.UserID}");
            } else {
                CurrentEntries = new List<DiaryEntry>();
            }
            return true;
        }

        /// <summary>
        /// Exports current user's diary entries
        /// </summary>
        /// <returns>Indication of success</returns>
        public bool ExportEntries() {
            if (CurrentUser == null) {
                return false;
            }
            EntrySaver.SerializeToXmlFile(CurrentEntries, $"Diary-Of-{CurrentUser.Username}");
            return true;
        }
    }
}
