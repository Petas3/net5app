using System.Runtime.Serialization;

namespace net5app
{
    [DataContract]
    public class DataMetric
    {
        [DataMember]
        public string Name;
        [DataMember]
        public double Value;

        public DataMetric(string Name, double Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
}