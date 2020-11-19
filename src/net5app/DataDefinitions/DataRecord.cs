using System.Runtime.Serialization;

namespace net5app
{
    [DataContract]
    public class DataRecord
    {
        [DataMember]
        public string Name;
        [DataMember]
        public byte[] SubjectValues;
        [DataMember]
        public byte[] MetricValues;
    }
}