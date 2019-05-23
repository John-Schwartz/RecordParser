using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RecordParser
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0) return;

                var objectList = new List<Record>();
                var helper = new ParseHelper();

                foreach(string file in args)
                {
                    var recordList = helper.ReadFileAndSplitLines(file).ToList(); // Read each line, split by delimiters, return cleaned up string array
                    recordList.ForEach(strArray => objectList.Add(new Record(strArray))); // for each record data string array, add to list
                }

                helper.PrintResults(objectList);//Outputs sorted with linq then call Console.WriteLine on the formatted string;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception thrown. \n{e.Message}\n{e.InnerException}");
            }
        }
    }

    
}

