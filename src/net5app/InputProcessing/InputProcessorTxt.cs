using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace net5app
{
    internal class InputProcessorTxt : IInputProcessor
    {
        private readonly string path;
        private readonly Collection<DataSubject> subjects;

        internal InputProcessorTxt(string path)
        {
            this.path = path;
            this.subjects = new Collection<DataSubject>
            {
                new DataSubject("Math", 40.0, 0, 100),
                new DataSubject("Physics", 35.0, 0, 100),
                new DataSubject("English", 25.0, 0, 100)
            };
        }

        /// <summary>
        /// Returns true on success, record is returned via out
        /// </summary>
        /// <param name="line"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        private bool ReadRecord(string line, out DataRecord record)
        {
            string[] fields = line.Split(";");
            record = null;

            //Check fields count
            if (fields.Length != subjects.Count + 1)
                return false;

            //Check name not contain =
            if (fields[0].Contains("="))
                return false;

            //Check name empty
            string name = fields[0].Trim();
            if (string.IsNullOrEmpty(name))
                return false;

            //Check other fields contain =
            for (int i = 1; i < fields.Length; i++)
                if (!fields[i].Contains("="))
                    return false;

            //Assign new record
            record = new DataRecord
            {
                //Read name
                Name = name
            };

            //Read subjects
            for (int i = 1; i < fields.Length; i++)
            {
                string[] f2 = fields[i].Trim().Split("=");
                
                if (f2.Length != 2)
                    return false;

                string f2Name = f2[0].Trim();
                string f2Value = f2[1].Trim();

                if (string.IsNullOrEmpty(f2Name))
                    return false;
                if (string.IsNullOrEmpty(f2Value))
                    return false;

                if (!subjects.Where(s => s.Name == f2Name).Any() || record.SubjectValues.ContainsKey(f2Name))
                    return false;
                if (!byte.TryParse(f2Value, out byte valOut))
                    return false;

                DataSubject subj = subjects.Where(s => s.Name == f2Name).First();
                if (!(valOut >= subj.MinAllowedValue && valOut <= subj.MaxAllowedValue))
                    return false;

                record.SubjectValues.Add(f2Name, valOut);
            }

            return true;
        }

        public DataStruct Process()
        {
            DataStruct rt = new DataStruct
            {
                //Assign subjects
                Subjects = subjects
            };

            //Read input file, read all groups
            const int BufferSize = 4096; //4K standard NTFS
            using (FileStream fileStream = File.OpenRead(path))
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string line;

                //Read group
                //If group has only label and no records its not a group but a header (if its not already set - in that case its invalid group)
                //Else its a group so assign it
                                
                DataGroup dg = null;
                bool isInGroup = false;
                bool obtainedTitle = false;
                int currLineIdx = 0;

                while ((line = streamReader.ReadLine()) != null)
                {
                    //Trim line for unexpected whitespace
                    line = line.Trim();

                    if (isInGroup == false)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            //Ignore empty line in this state
                        }
                        else
                        {
                            //Enter group by reading label
                            isInGroup = true;
                            dg = new DataGroup();
                            if (line.Contains(";"))
                            {
                                //Try read data directly
                                dg.Label = "Unknown";
                                if (ReadRecord(line, out DataRecord record))
                                    dg.Records.Add(record);
                                else
                                    dg.InvalidRecordsLines.Add(line);
                            }
                            else
                                dg.Label = line;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            //Check group empty
                            if (dg.Records.Count == 0)
                            {
                                if (!obtainedTitle && currLineIdx < 3)
                                {
                                    obtainedTitle = true;
                                    rt.Header = dg.Label;
                                    //Reset group
                                    isInGroup = false;
                                    dg = null;
                                }
                                else
                                {
                                    //Invalid or empty group == invalid as well
                                    rt.InvalidGroupsLines.Add(dg.Label);
                                }
                            }
                            else
                            {
                                //Close and output group
                                rt.Groups.Add(dg);
                                isInGroup = false;
                                dg = null;
                            }
                        }
                        else
                        {
                            //Read data records
                            if (ReadRecord(line, out DataRecord record))
                                dg.Records.Add(record);
                            else
                                dg.InvalidRecordsLines.Add(line);
                        }
                    }

                    currLineIdx++;
                }
            }
            
            //In input processing only groups and subject are assigned
            return rt; 
        }
    }
}