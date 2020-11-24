using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace net5app
{
    [DataContract]
    public class DataSubject
    {
        [DataMember(Name = "name")]
        [XmlElement(ElementName = "name")]
        public string Name;
        [DataMember(Name = "weight")]
        [XmlElement(ElementName = "weight")]
        public double Weight;
        [IgnoreDataMember]
        [XmlIgnore]
        public byte MinAllowedValue;
        [IgnoreDataMember]
        [XmlIgnore]
        public byte MaxAllowedValue;

        public DataSubject() { }
        public DataSubject(string Name, double Weight, byte MinAllowedValue, byte MaxAllowedValue)
        {
            this.Name = Name;
            this.Weight = Weight;
            this.MinAllowedValue = MinAllowedValue;
            this.MaxAllowedValue = MaxAllowedValue;
        }
    }
}