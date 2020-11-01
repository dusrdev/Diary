using System;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

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
            HashedPassword = Aes.GeneratePassword(Password);
        }

        /// <summary>
        /// Serialization function
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("UserID", UserID);
            info.AddValue("Username", Username);
            info.AddValue("HashedPassword", HashedPassword);
        }

        /// <summary>
        /// Deserialization function
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public User(SerializationInfo info, StreamingContext ctxt) {
            UserID = (int)info.GetValue("UserID", typeof(int));
            Username = (string)info.GetValue("Username", typeof(string));
            HashedPassword = (string)info.GetValue("HashedPassword", typeof(string));
        }
    }
}
