using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace net5app
{
    [DataContract]
    public class DataGroup
    {
        [DataMember(Name = "label")]
        [XmlElement(ElementName = "label")]
        public string Label;
        [DataMember(Name = "records")]
        [XmlArray(ElementName = "records")]
        public Collection<DataRecord> Records;
        [DataMember(Name = "invalid")]
        [XmlArray(ElementName = "invalid")]
        public Collection<string> InvalidRecordsLines;
        [DataMember(Name = "stats")]
        [XmlArray(ElementName = "stats")]
        public Collection<DataStatistics> Statistics;

        public DataGroup()
        {
            Label = string.Empty;
            Records = new Collection<DataRecord>();
            InvalidRecordsLines = new Collection<string>();
            Statistics = new Collection<DataStatistics>();
        }
    }
}