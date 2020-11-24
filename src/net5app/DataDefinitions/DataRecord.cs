using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace net5app
{
    [DataContract]
    public class DataRecord
    {
        [DataMember (Name = "name")]
        [XmlElement (ElementName = "name")]
        public string Name;
        [IgnoreDataMember]
        [XmlIgnore]
        public Dictionary<string, byte> SubjectValues;
        [IgnoreDataMember]
        [XmlIgnore]
        public Dictionary<string, byte> MetricValues;
        [DataMember(Name = "subjects")]
        [XmlArray(ElementName = "subjects")]
        public Collection<DataKeyValue<byte>> Subjects
        {
            get
            {
                return SubjectValues.FromDictionary();
            }
        }
        [DataMember(Name = "metrics")]
        [XmlArray(ElementName = "metrics")]
        public Collection<DataKeyValue<byte>> Metrics
        {
            get
            {
                return MetricValues.FromDictionary();
            }
        }

        public DataRecord()
        {
            Name = string.Empty;
            SubjectValues = new Dictionary<string, byte>();
            MetricValues = new Dictionary<string, byte>();
    }
    }
        
    internal static class DataRecordExtension
    {
        /// <summary>
        /// Mines data values array for further processing from data loaded collection
        /// </summary>
        /// <param name="e">Collection</param>
        /// <param name="subjectName">Subject name</param>
        /// <returns></returns>
        internal static byte[] GetSubjectValues(this Collection<DataRecord> e, string subjectName)
        {
            return e.Select(x => x.SubjectValues[subjectName]).ToArray();
        }

        /// <summary>
        /// Mines data values array for further processing from data loaded collection
        /// </summary>
        /// <param name="e">Collection</param>
        /// <param name="metricName">Metric name</param>
        /// <returns></returns>
        internal static byte[] GetMetricValues(this Collection<DataRecord> e, string metricName)
        {
            return e.Select(x => x.MetricValues[metricName]).ToArray();
        }
    }
}