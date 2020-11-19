using System.Collections.Generic;
using System.Runtime.Serialization;

namespace net5app
{
    [DataContract]
    public class DataStruct
    {
        [DataMember]
        public string Header;
        [DataMember]
        public List<DataSubject> Subjects;
        [DataMember]
        public List<DataGroup> Groups;
        [DataMember]
        public List<DataGroup> InvalidGroups;
    }
}