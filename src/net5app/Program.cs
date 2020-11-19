using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace net5app
{
    public class Program
    {
        /// <summary>
        /// Main program entrypoint
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //Determine arguments
            //1st argument is required [filename]
            //2nd argument is optional [-json]

            if (args.Length == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Help");
                Console.WriteLine("[-json] Switch to json output, default is xml");
                Console.WriteLine("[filename] Unlimited arguments considered as input files, relative or absolute paths");
            }
            else
            {
                //Detect json flag
                bool json = args.Contains("-json");
                
                //Check if arg is a valid file
                Collection<string> inputPaths = new Collection<string>();
                foreach (string arg in args)
                    if (File.Exists(arg))
                        inputPaths.Add(arg);

                //Startup
                Console.WriteLine();
                Console.WriteLine("Startup");

                if (json)
                    Console.WriteLine("Outformat=Json");
                else
                    Console.WriteLine("Outformat=Xml");
                foreach (string path in inputPaths)
                    Console.WriteLine("InFile=" + path);

                //Processing
                Console.WriteLine();
                Console.WriteLine("Processing");

                IEnumerable<Thread> threads = new Collection<Thread>();
                if (json)
                {
                    foreach (string path in inputPaths)
                        threads = threads.Append(ProcessFileStart(new InputProcessorTxt(path), new OutputProcessorJson(Path.ChangeExtension(path, "json"))));
                }
                else
                {
                    foreach (string path in inputPaths)
                        threads = threads.Append(ProcessFileStart(new InputProcessorTxt(path), new OutputProcessorXml(Path.ChangeExtension(path, "xml"))));
                }
                
                //Wait for finish
                while (threads.Any())
                {
                    threads = threads.Where(t => t.IsAlive);
                    Thread.Sleep(20);
                }

                //Wait
                Console.WriteLine();
                Console.WriteLine("Press key to exit ..");
                Console.ReadKey();
            }
        }

        private static Thread ProcessFileStart(IInputProcessor iP, IOutputProcessor oP)
        {
            Thread t = new Thread(() => ProcessFile(iP, oP));
            t.Start();
            return t;
        }

        private static void ProcessFile(IInputProcessor iP, IOutputProcessor oP)
        {
            DataStruct data = iP.Process();
            oP.Process(data);
        }
    }
}