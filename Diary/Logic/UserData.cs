using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Logic {
    [Serializable()]
    public class UserData : ISerializable{
        public int UserID { get; set; }
        public List<DiaryEntree> UserEntrees { get; set; }

        public UserData() { }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("UserID", UserID);
            info.AddValue("UserEntrees", UserEntrees);
        }

        //TODO: Add deserialization function
    }
}
