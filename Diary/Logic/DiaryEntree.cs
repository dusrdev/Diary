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
        /// <param name="entreeID">ID of the last entree before this one</param>
        public DiaryEntree(string title, string body, int entreeID) {
            Title = title.ToTitle();
            Body = body;
            Date = DateTime.Now.FullDateAndTime();
            ID = entreeID;
        }

        /// <summary>
        /// Serialization function
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("ID", ID);
            info.AddValue("Date", Date);
            info.AddValue("Title", Title);
            info.AddValue("Body", Body);
        }

        /// <summary>
        /// Deserialization function
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public DiaryEntree(SerializationInfo info, StreamingContext ctxt) {
            ID = (int)info.GetValue("ID", typeof(int));
            Date = (string)info.GetValue("Date", typeof(string));
            Title = (string)info.GetValue("Title", typeof(string));
            Body = (string)info.GetValue("Body", typeof(string));
        }

        public override string ToString() {
            return "Not implemented to listbox";
        }
    }
}
