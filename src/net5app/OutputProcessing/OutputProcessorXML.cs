using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace net5app
{
    internal class OutputProcessorXml : IOutputProcessor
    {
        private readonly string path;

        internal OutputProcessorXml(string path)
        {
            this.path = path;
        }

        public void Process(DataStruct data)
        {
            XmlSerializer ser = new XmlSerializer(typeof(DataStruct));
            using StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(false));
            ser.Serialize(sw.BaseStream, data);
        }
    }
}