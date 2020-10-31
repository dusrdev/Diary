using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Logic {
    [Serializable()]
    public class DiaryEntree : ISerializable {
        public int ID { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public DiaryEntree() { }

        /// <summary>
        /// Initialization function
        /// </summary>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="previousEntreeID">ID of the last entree before this one</param>
        public DiaryEntree(string title, string body, int previousEntreeID) {
            Title = title; //TODO: Format as title
            Body = body;
            //TODO: DateTime as FullDateAndTime() string
            ID = previousEntreeID + 1;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("ID", ID);
            info.AddValue("Date", Date);
            info.AddValue("Title", Title);
            info.AddValue("Body", Body);
        }

        //TODO: Add deserialization function
    }
}
