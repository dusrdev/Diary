using System;
using System.Runtime.Serialization;

namespace Diary.Logic {
    [Serializable()]
    public class DiaryEntry : ISerializable {
        public int ID { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public DiaryEntry() { }

        /// <summary>
        /// Initialization function
        /// </summary>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="entryId">ID of the last entry before this one</param>
        public DiaryEntry(string title, string body, int entryId) {
            Title = title.ToTitle();
            Body = body;
            Date = DateTime.Now.FullDateAndTime();
            ID = entryId;
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
        /// De-serialization function
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public DiaryEntry(SerializationInfo info, StreamingContext ctxt) {
            ID = (int)info.GetValue("ID", typeof(int));
            Date = (string)info.GetValue("Date", typeof(string));
            Title = (string)info.GetValue("Title", typeof(string));
            Body = (string)info.GetValue("Body", typeof(string));
        }
    }
}
