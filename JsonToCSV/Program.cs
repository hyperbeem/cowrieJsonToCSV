using System;
using System.IO;
using Newtonsoft.Json;
using CsvHelper;

using System.Collections.Generic;

namespace JsonToCSV
{
    class Program
    {
        public static string dirPath;
        public static string outputPath;
        public static string[] files;

        public static string LOGINFAILED = @"cowrie.login.failed";
        public static List<Attempt> accessList;
        public static List<AttemptTrim> attemptTrims;

        static void Main(string[] args)
        {
            //Setup();

            Console.WriteLine("William Savage, Spoons™");
            Console.WriteLine("Enter directory path");

            string t = Console.ReadLine();

            Console.WriteLine("Enter output directory");

            string o = Console.ReadLine();

            if (Directory.Exists(t))
            {
                dirPath = t;
                if (Directory.Exists(o))
                {
                    outputPath = o;
                    Setup();
                }
                else
                {
                    throw new Exception("ERROR: Output directory does not exist!");
                }
            }
            else
            {
                throw new Exception("ERROR: Directory does not exist!");
            }
        }

        public static void Setup()
        {
            files = Directory.GetFiles(dirPath);
            Console.WriteLine("Number of files: " + files.Length);

            int c = 0;
            string ln;
            accessList = new List<Attempt>();
            attemptTrims = new List<AttemptTrim>();

            foreach (string s in files)
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    while ((ln = sr.ReadLine()) != null)
                    {
                        if (ln.Contains(LOGINFAILED))
                        {
                            accessList.Add(JsonConvert.DeserializeObject<Attempt>(ln));
                            c++;
                        }
                    }
                }
            }

            Console.WriteLine("Loaded {0} access attempts", c);
            Trim();
            CSVExport(c);

            Console.Read();
        }


        public static void Trim()
        {
            foreach(Attempt a in accessList)
            {
                AttemptTrim t = new AttemptTrim()
                {
                    timestamp = a.timestamp,
                    session = a.session,
                    src_ip = a.src_ip,
                    username = a.username,
                    password = a.password
                };

                attemptTrims.Add(t);
            }
            accessList.Clear();
        }

        public static void CSVExport(int count)
        {
            string co = outputPath + "\\" + count.ToString() + ".csv";

            using (StreamWriter sw = new StreamWriter(co))
            {
                using (CsvWriter csv = new CsvWriter(sw))
                {
                    csv.Configuration.HasHeaderRecord = true;
                    csv.WriteRecords(attemptTrims);
                }
            }
        }
    }
}
