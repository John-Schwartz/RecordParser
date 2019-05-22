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
                var objectList = new List<Person>();
                var helper = new ParseHelper();
                var delimArray = new char[] { '|', ',', ' ' };
                var filePath = "D:\\Users\\john.schwartz\\source\\repos\\RecordParser\\RecordParser\\RecordFile1.txt";

                Console.WriteLine("Start");

                // Read each line of the the file, split the data by the delimiters, return the cleaned up string array
                var recordList = helper.ReadFileAndSplitLines(filePath, delimArray).ToList();
                // for each record data string array, add it to the list
                recordList.ForEach(strArray => objectList.Add(new Person(strArray)));

                //Outputs sorted with linq then call Console.WriteLine on the formatted string;
                helper.PrintResults(objectList);

                Console.WriteLine("\n\n Finished");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception thrown. \n{e.Message}\n{e.InnerException}");
            }
        }
                        
        public static void WriteExceptionMessage(Exception e, string functionName = "", string humanMessage = "")
        {
            Console.WriteLine($"{functionName} - {humanMessage}\n");
            Console.WriteLine(e.Message);
        }

    }

    
}

