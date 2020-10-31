using System;
using System.Runtime.Serialization;

namespace Diary.Logic {
    [Serializable()]
    public class User : ISerializable {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public string Password { get; set; }

        public User() { }

        /// <summary>
        /// Initializer
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password">not hashed</param>
        public User(string username, string password) {
            Username = username;
            Password = password;
            //TODO: Add password hashing
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("UserID", UserID);
            info.AddValue("Username", Username);
            info.AddValue("HashedPassword", HashedPassword);
        }

        //TODO: Add deserialization function
    }
}
