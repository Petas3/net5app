using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace net5app
{
    [DataContract]
    public class DataStruct
    {
        [DataMember(Name = "title")]
        [XmlElement(ElementName = "title")]
        public string Header;
        [DataMember(Name = "subjects")]
        [XmlArray(ElementName = "subjects")]
        public Collection<DataSubject> Subjects;
        [DataMember(Name = "groups")]
        [XmlArray(ElementName = "groups")]
        public Collection<DataGroup> Groups;
        [DataMember(Name = "invalid")]
        [XmlArray(ElementName = "invalid")]
        public Collection<string> InvalidGroupsLines;
        [DataMember(Name = "stats")]
        [XmlArray(ElementName = "stats")]
        public Collection<DataStatistics> Statistics;

        public DataStruct()
        {
            Header = string.Empty;
            Subjects = new Collection<DataSubject>();
            Groups = new Collection<DataGroup>();
            InvalidGroupsLines = new Collection<string>();
            Statistics = new Collection<DataStatistics>();
        }
    }
}