using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Diary.Logic {
    [Serializable()]
    public class UserData : ISerializable{
        public int UserID { get; set; }
        public List<DiaryEntree> UserEntrees { get; set; }

        /// <summary>
        /// Empty constructor - required for serialization
        /// </summary>
        public UserData() { }

        /// <summary>
        /// Serialization function
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("UserID", UserID);
            info.AddValue("UserEntrees", UserEntrees);
        }

        /// <summary>
        /// Deserialization fucntion
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public UserData(SerializationInfo info, StreamingContext ctxt) {
            UserID = (int)info.GetValue("UserID", typeof(int));
            UserEntrees = (List<DiaryEntree>)info.GetValue("UserEntrees", typeof(List<DiaryEntree>));
        }
    }
}
