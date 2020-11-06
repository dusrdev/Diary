﻿using System.Collections.Generic;
using System.Linq;
using static Diary.Logic.Logger;

namespace Diary.Logic {
    public class ActiveDatabase {
        public List<DiaryEntree> CurrentEntrees { get; set; }
        public List<User> Udb { get; set; }
        public User CurrentUser { get; set; }
        public MySerializer EntreeSaver { get; set; }
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
            EntreeSaver = new MySerializer(CurrentUser.Password);
            return true;
        }

        /// <summary>
        /// Search entrees by query
        /// </summary>
        /// <param name="query">search query</param>
        /// <returns>Results (Not found if empty)</returns>
        public List<DiaryEntree> Search(string query) {
            var results = new List<DiaryEntree>();
            if (CurrentEntrees == null || CurrentEntrees.Count == 0) {
                return results;
            }
            var sQuery = query.Queryable();
            if (query.HasBorders("[", "]")) {
                sQuery = query.Subset(1, 1).Queryable();
                return CurrentEntrees.FindAll(e => e.Date.QuerySearch(sQuery));
            }
            foreach (var entree in CurrentEntrees) {
                if (entree.Title.QuerySearch(sQuery) || entree.Body.QuerySearch(sQuery)) {
                    results.Add(entree);
                }
            }
            return results;
        }

        /// <summary>
        /// Remove entree by id (displayed in search window)
        /// </summary>
        /// <param name="queryId">ID of desired entree</param>
        /// <returns>indication of success</returns>
        public bool RemoveEntree(int queryId) {
            if (!CurrentEntrees.Exists(e => e.ID == queryId)) {
                return false;
            }
            CurrentEntrees.RemoveAll(e => e.ID == queryId);
            for (var i = 0; i < CurrentEntrees.Count; i++) {
                CurrentEntrees[i].ID = i;
            }
            EntreeSaver.SerializeToFile(CurrentEntrees, $"Edb{CurrentUser.UserID}");
            return true;
        }

        /// <summary>
        /// Add entree
        /// </summary>
        /// <param name="title">Entree title</param>
        /// <param name="body">Entree body</param>
        /// <returns>indication of success</returns>
        public bool AddEntree(string title, string body) {
            CurrentEntrees ??= new List<DiaryEntree>();
            var cleanBody = body.RemoveWhiteSpace();
            if (CurrentEntrees.Exists(e => e.Body.RemoveWhiteSpace() == cleanBody)) {
                return false;
            }
            var newEntreeId = CurrentEntrees.Count > 0 ? CurrentEntrees.Last().ID + 1 : 0;
            var newEntree = new DiaryEntree(title, body, newEntreeId);
            CurrentEntrees.Add(newEntree);
            EntreeSaver.SerializeToFile(CurrentEntrees, $"Edb{CurrentUser.UserID}");
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
            EntreeSaver = new MySerializer(CurrentUser.Password);
            if (Native.DoesFileExist($"Edb{CurrentUser.UserID}")) {
                CurrentEntrees = EntreeSaver.DeserializeFromFile<List<DiaryEntree>>($"Edb{CurrentUser.UserID}");
            } else {
                CurrentEntrees = new List<DiaryEntree>();
            }
            return true;
        }

        /// <summary>
        /// Exports current user's diary entrees
        /// </summary>
        /// <returns>Indication of success</returns>
        public bool ExportEntrees() {
            if (CurrentUser == null) {
                return false;
            }
            EntreeSaver.SerializeToXmlFile(CurrentEntrees, $"Diary-Of-{CurrentUser.Username}");
            return true;
        }
    }
}
