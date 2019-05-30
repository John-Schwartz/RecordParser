using System;
using System.Collections.Generic;
using System.Linq;
using ParseHelperLibrary;

namespace RecordParser
{
    class Program
    {
        public static void Main(string[] args)
        {
            // if no filepath args passed, write why program failed and return
            if (!args.Any())
            {
                Console.WriteLine("No filepath parameters passed. Press any key to exit.");
                Console.ReadKey();
                return;
            }

            var objectList = new List<Record>();
            var helper = new ParseHelper();

            // Read each line, split by delimiters, return cleaned up string array
            var recordList = helper.ReadFileAndSplitLines(args).ToList();

            // for each record data string array, add to list
            recordList.ForEach(strCollection => objectList.Add(Record.CreateRecord(strCollection)));

            // Print results according to specified sort orders
            helper.PrintResults(objectList);

            Console.ReadKey();
        }
    }
}

