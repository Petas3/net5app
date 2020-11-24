using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace net5app
{
    [DataContract]
    public class DataMetric
    {
        [DataMember(Name = "name")]
        [XmlElement(ElementName = "name")]
        public string Name;
        [DataMember(Name = "value")]
        [XmlElement(ElementName = "value")]
        public byte Value;

        public DataMetric(string Name, byte Value)
        {
            this.Name = Name;
            this.Value = Value;            
        }
    }
}