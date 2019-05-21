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
            var objectList = new List<Person>();
            var helper = new ParseHelper();
            var delimArray = new char[] { '|', ',', ' ' };
            var filePath = "D:\\Users\\john.schwartz\\source\\repos\\RecordParser\\RecordParser\\RecordFile1.txt";            

            Console.WriteLine("Start");
            
            // Read each line of the the file, split the data by the delimiters, return the cleaned up string array
            var recordList = helper.ReadFileAndSplitLines(filePath, delimArray).ToList();
            recordList.ForEach(strArray => objectList.Add(new Person(strArray))); // for each record data string array, add it to the list

            //Outputs sort with linq then call Console.Write on the formatted string;
            Output1(objectList);
            Console.WriteLine();
            Output2(objectList);
            Console.WriteLine();
            Output3(objectList);

            Console.WriteLine("\n\n Finished");
            Console.ReadKey();
        }

        public static void Output1(List<Person> personList)
        {
            try
            {
                var sortedList = (from p in personList
                                  orderby p.Gender, p.LastName ascending
                                  select p).ToList();
                sortedList.ForEach(x => Console.Write(x.GetFormattedString()));
            }
            catch (Exception e)
            {
                var message = "Exception thrown while writing Output1";
                WriteExceptionMessage(e, "Output1", message);
            }
        }

        public static void Output2(List<Person> personList)
        {
            try
            {
                var sortedList = (from p in personList
                                  orderby p.DateOfBirth ascending
                                  select p).ToList();
                sortedList.ForEach(x => Console.Write(x.GetFormattedString()));
            }
            catch (Exception e)
            {
                var message = "Exception thrown while writing Output2";
                WriteExceptionMessage(e, "Output2", message);
            }
        }

        public static void Output3(List<Person> personList)
        {
            try
            {
                //var sortedList2 = personList.Select(x => x).OrderBy(x => x.LastName).ToList();
                var sortedList = (from p in personList
                                  orderby p.LastName descending
                                  select p).ToList();

                sortedList.ForEach(x => Console.Write(x.GetFormattedString()));
            }
            catch (Exception e)
            {
                var message = "Exception thrown while writing Output3";
                WriteExceptionMessage(e, "Output3", message);
            }
        }
                
        public static void WriteExceptionMessage(Exception e, string functionName = "", string humanMessage = "")
        {
            Console.WriteLine($"{functionName} - {humanMessage}\n");
            Console.WriteLine(e.Message);
        }

    }

    
}

