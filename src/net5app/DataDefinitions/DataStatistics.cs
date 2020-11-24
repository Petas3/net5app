using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace net5app
{
    [DataContract]
    public class DataStatistics
    {
        [DataMember(Name = "name")]
        [XmlElement(ElementName = "name")]
        public string Name;
        [DataMember(Name = "average")]
        [XmlElement(ElementName = "average")]
        public double? Average;
        [DataMember(Name = "modus")]
        [XmlArray(ElementName = "modus")]
        public Collection<byte> Modus;
        [DataMember(Name = "median")]
        [XmlElement(ElementName = "median")]
        public double? Median;

        public DataStatistics() { }

        public DataStatistics(string Name, double? Average, Collection<byte> Modus, double? Median)
        {
            this.Name = Name;
            this.Average = Average;
            this.Modus = Modus;
            this.Median = Median;
        }

        public DataStatistics(string Name, byte[] array, byte minValue, byte maxValue)
        {
            this.Name = Name;
            this.Average = DataMath.GetAverage(array);
            this.Modus = DataMath.GetModus(array, minValue, maxValue);
            this.Median = DataMath.GetMedian(array);
        }
    }
}