using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace net5app
{
    [DataContract]
    public class DataKeyValue<T>
    {
        [DataMember(Name = "key")]
        [XmlElement(ElementName = "key")]
        public string key;
        [DataMember(Name = "value")]
        [XmlElement(ElementName = "value")]
        public T value;

        public DataKeyValue() { }
        public DataKeyValue(string key, T value) 
        {
            this.key = key;
            this.value = value;
        }
    }

    internal static class DataKeyValueExtension
    {
        internal static Collection<DataKeyValue<T>> FromDictionary<T>(this Dictionary<string, T> e)
        {
            Collection<DataKeyValue<T>> rt = new Collection<DataKeyValue<T>>();
            foreach (var i in e.Keys)
                rt.Add(new DataKeyValue<T>(i, e[i]));
            return rt;
        }
    }
}
