using System;
using System.Collections.Generic;
using System.Linq;
using static Diary.Logic.Logger;

namespace Diary.Logic {
    public class ActiveDatabase {
        public List<DiaryEntree> CurrentEntrees { get; set; }
        public Edb EntreeDatabase { get; set; }
        public List<User> Udb { get; set; }
        public User CurrentUser { get; set; }
        public MySerializer MS { get; set; }

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
            try {
                newUser.UserID = Udb.Last().UserID + 1;
                Udb.Add(newUser);
                MS.SerializeToFile(Udb, "Udb");
            } catch (Exception e) {
                Log(e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Essentially a login function
        /// </summary>
        /// <param name="userInfo">login credentials</param>
        /// <returns>indication of success</returns>
        public bool LoadUser(User userInfo) {
            foreach (var user in Udb) {
                if (user.Username == userInfo.Username && true) { //TODO: Check password using AES
                    CurrentUser = userInfo;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Search entrees by query
        /// </summary>
        /// <param name="query">search query</param>
        /// <returns>Results (Not found if empty)</returns>
        public List<DiaryEntree> Search(string query) {
            List<DiaryEntree> results = new List<DiaryEntree>();
            var sQuery = query.Queryable();
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
        /// <param name="queryID">ID of desired entree</param>
        /// <returns>indication of success</returns>
        public bool RemoveEntree(int queryID) {
            try {
                CurrentEntrees.RemoveAll(e => e.ID == queryID); //TODO: Check for need
                EntreeDatabase.TotalEntrees[CurrentUser.UserID].RemoveAll(e => e.ID == queryID);
                MS.SerializeToFile(EntreeDatabase, "Edb");
            } catch (Exception e) {
                Log(e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Add entree
        /// </summary>
        /// <param name="newEntree">initialize before sending as argument</param>
        /// <returns>indication of success</returns>
        public bool AddEntree(DiaryEntree newEntree) {
            try {
                CurrentEntrees.Add(newEntree); //TODO: Check for need
                EntreeDatabase.TotalEntrees[CurrentUser.UserID].Add(newEntree);
                MS.SerializeToFile(EntreeDatabase, "Edb");
            } catch (Exception e) {
                Log(e);
                return false;
            }
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
            try {
                Udb.RemoveAll(u => u.UserID == CurrentUser.UserID);
                EntreeDatabase.TotalEntrees.Remove(CurrentUser.UserID);
                MS.SerializeToFile(Udb, "Udb");
                MS.SerializeToFile(EntreeDatabase, "Edb");
            } catch (Exception e) {
                Log(e);
                return false;
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
            try {
                MS.SerializeToXmlFile(CurrentEntrees, $"{CurrentUser.Username}-Diary");
            } catch (Exception e) {
                Log(e);
                return false;
            }
            return true;
        }
    }
}
