using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordParser
{
    class Program
    {
        public static void Main(string[] args)
        {
            var helper = new ParseHelper();
            var filePath = "D:\\Users\\john.schwartz\\source\\repos\\RecordParser\\RecordParser\\RecordFile1.txt";
            var delimArray = new char[] { '|', ',', ' ' };

            Console.WriteLine("Start");
            var objectList = new List<Person>();
            
            var recordList = helper.ReadFileAndSplitLines(filePath, delimArray).ToList();
                        
            for (var i = 0; i < recordList.Count; i++)
            {
                objectList.Add(new Person(recordList[i]));
                //objectList.ForEach(x => );
            }

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
                sortedList.ForEach(x => WriteFormattedRecord(x));
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
                sortedList.ForEach(x => WriteFormattedRecord(x));
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

                sortedList.ForEach(x => WriteFormattedRecord(x));
            }
            catch (Exception e)
            {
                var message = "Exception thrown while writing Output3";
                WriteExceptionMessage(e, "Output3", message);
            }
        }

        public static void WriteFormattedRecord(Person person)
        {

            Console.WriteLine("Name: {0,-30} | Gender: {1,-7} | Favorite Color: {2,-15} | DOB: {3,-10}",
                                    $"{person.LastName}, {person.FirstName}",
                                    person.Gender,
                                    person.FavoriteColor,
                                    person.DateOfBirth.ToString("M/d/yyyy"));
        }

        public static void WriteExceptionMessage(Exception e, string functionName = "", string humanMessage = "")
        {
            Console.WriteLine($"{functionName} - {humanMessage}\n");
            Console.WriteLine(e.Message);
        }

    }

    
}

