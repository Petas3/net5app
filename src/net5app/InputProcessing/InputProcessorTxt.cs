using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net5app
{
    internal class InputProcessorTxt : IInputProcessor
    {
        private readonly string path;
        private readonly List<DataSubject> subjects;

        internal InputProcessorTxt(string path)
        {
            this.path = path;
            this.subjects = new List<DataSubject>
            {
                new DataSubject("Math", 40.0),
                new DataSubject("Physics", 35.0),
                new DataSubject("English", 25.0)
            };
        }

        public DataStruct Process()
        {
            DataStruct rt = new DataStruct
            {
                //Assign subjects
                Subjects = subjects,
                Groups = new List<DataGroup>()
            };

            //Read input file, read all groups
            const int BufferSize = 4096;
            using (FileStream fileStream = File.OpenRead(path))
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string line;

                //Read group

                //If group has only label and no records its not a group but a header (if its not already set - in that case its invalid group)

                //Else its a group so assign it

                while ((line = streamReader.ReadLine()) != null)
                {

                }
            }
            
            //In input processing only groups and subject are assigned
            return rt; 
        }
    }
}