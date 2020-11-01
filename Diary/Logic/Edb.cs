using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Logic {
    [Serializable()]
    public class Edb : ISerializable {
        public Dictionary<int, List<DiaryEntree>> TotalEntrees { get; set; }

        /// <summary>
        /// Empty constuctor - required for serialization
        /// </summary>
        public Edb() { }

        /// <summary>
        /// Serialization function
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("TotalEntrees", TotalEntrees);
        }

        /// <summary>
        /// Deserialization function
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public Edb(SerializationInfo info, StreamingContext ctxt) {
            TotalEntrees = (Dictionary<int, List<DiaryEntree>>)info.GetValue("TotalEntrees", typeof(Dictionary<int, List<DiaryEntree>>));
        }
    }
}
