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
            Console.WriteLine("Start");
            var objectList = new List<Person>();            
            var filePath = "D:\\Users\\john.schwartz\\source\\repos\\RecordParser\\RecordParser\\RecordFile1.txt";
            var delimArray = new char[] { '|', ',', ' ' };

            var recordDictionary = ReadFileAndSplitByDelim(filePath, delimArray);

            for (var i = 0; i < recordDictionary.Count; i++)
            {
                objectList.Add(MakePersonFromStringList(recordDictionary[i]));
            }

            // read file, split by line => List<string>
            //// make generic, 3 file types

            // Validate and trim safe strings and parse date, then make Person obj.
            //// If line not valid, add to 'invalidList' -- separate by some escape character maybe? Or separate files?

            // Once all people have been made, dump to file in display formats or just display on console?

            // Has to be available for api

            //var pipeDelimLines = File.ReadAllLines("");
            //var commaDelimStringList = new List<string>();
            //var spaceDelimStringList = new List<string>();

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

                sortedList.ForEach(x => WriteFormattedRecord(x) );
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

        public static string SafeString(object obj, bool trimString = true)
        {
            try
            {
                if (trimString) return obj?.ToString().Trim() ?? string.Empty;
                return obj?.ToString() ?? string.Empty;
            }
            catch (Exception e)
            {
                var message = "Exception thrown while making string safe";
                WriteExceptionMessage(e, "SafeString", message);
            }

            return string.Empty;
        }

        public static Person MakePersonFromStringList(List<string> recordStringFields)
        {
            var newPerson = new Person();
            
            try
            {
                
                

                newPerson = new Person
                {
                    LastName = SafeString(recordStringFields[0]),
                    FirstName = SafeString(recordStringFields[1]),
                    Gender = SafeString(recordStringFields[2]),
                    FavoriteColor = SafeString(recordStringFields[3]),
                    DateOfBirth = ParseDateString(recordStringFields[4])
                };

            }
            catch (Exception e)
            {
                var message = "Exception thrown while creating Person object";
                WriteExceptionMessage(e, "MakePersonFromStringList", message);
                //recordStringFields.ForEach(x => Console.Write($"{SafeString(x)} "));
            }

            return newPerson;
        }

        public static Dictionary<int, List<string>> ReadFileAndSplitByDelim(string path, char[] delim)
        {
            if (path == null || path == string.Empty || !System.IO.File.Exists(path))
            {
                Console.WriteLine("Invalid path");
                return new Dictionary<int, List<string>>();
            }

            var recordArrayDict = new Dictionary<int, List<string>>();
            try
            {
                var allLines = File.ReadAllLines(path);

                var counter = 0;
                foreach (var recordString in allLines)
                {
                    if (recordString == string.Empty) { continue; }

                    var splitStringObject = recordString.Split(delim).ToList();
                    //TODO: Can't currently take dates of format "Feb 21 1990"
                    var DelimChars = splitStringObject.RemoveAll(x => x == " " || x == "|" || x == "," || x == string.Empty);

                    recordArrayDict.Add(counter, splitStringObject);
                    counter++;
                }
            }
            catch (Exception e)
            {
                var message = "Exception thrown while creating record dictionary";
                WriteExceptionMessage(e, "ReadFileAndSplitByDelim", message);
            }

            return recordArrayDict;
        }

        public static DateTime ParseDateString(string dateString)
        {
            DateTime.TryParse(dateString, out DateTime result);
            if (result == null || result == DateTime.MinValue)
            {
                return DateTime.MinValue;
            }
            return result;
        }

        public static void WriteExceptionMessage(Exception e, string functionName = "", string humanMessage = "")
        {
            Console.WriteLine($"{functionName} - {humanMessage}\n");
            Console.WriteLine(e.Message);
        }

    }

    public class Person
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string FavoriteColor { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
