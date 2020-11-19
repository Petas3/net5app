using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace net5app
{
    internal class OutputProcessorJson : IOutputProcessor
    {
        private readonly string path;

        internal OutputProcessorJson(string path)
        {
            this.path = path;
        }

        public void Process(DataStruct data)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(DataStruct));
            using StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(false));
            ser.WriteObject(sw.BaseStream, data);
        }
    }
}