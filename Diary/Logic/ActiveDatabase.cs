using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Logic {
    public class ActiveDatabase {
        public List<DiaryEntree> Entrees { get; set; }
        public List<User> Udb { get; set; }
        public User CurrentUser { get; set; }
        //TODO: Add serialization object

        /// <summary>
        /// Creates a new user from the login window
        /// </summary>
        /// <param name="newUser">Credentials</param>
        /// <returns>indication of success</returns>
        public bool AddUser(User newUser) {
            //TODO: Implement
            //Load all users from udb
            //Check if newUser exists by Username
            //If does exist, add and serialize.
            return false;
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
                    //TODO: Import this users entrees
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
            foreach (var entree in Entrees) {
                if (true) {
                    //TODO: Implement query search
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
                Entrees.RemoveAll(e => e.ID == queryID);
                //TODO: Serialzie
            } catch (Exception e) {
                //TODO: Log e
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
                Entrees.Add(newEntree);
                //TODO: Serialize
            } catch (Exception e) {
                //TODO: Log e
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
                //TODO: Serialize
                //TODO: Remove All of this users entrees by ID and also serialize
            } catch (Exception e) {
                //TODO: Log e
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
                //TODO: Export current user's diary entrees to xml
            } catch (Exception e) {
                //TODO: Log e
                return false;
            }
            return true;
        }
    }
}
