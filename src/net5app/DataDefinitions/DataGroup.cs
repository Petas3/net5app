using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace net5app
{
    [DataContract]
    public class DataGroup
    {
        [DataMember]
        public string Label;
        [DataMember]
        public Collection<DataRecord> Records;
        [DataMember]
        public Collection<DataRecord> InvalidRecords;
    }
}