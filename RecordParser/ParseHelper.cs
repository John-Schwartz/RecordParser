using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordParser
{
    public class ParseHelper
    {

        public static void Output1(List<Person> personList)
        {
            try
            {
                var sortedList = (from p in personList
                                  orderby p.Gender, p.LastName ascending
                                  select p).ToList();
                sortedList.ForEach(x => Console.WriteLine(x.GetFormattedString()));
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
                sortedList.ForEach(x => Console.WriteLine(x.GetFormattedString()));
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

                sortedList.ForEach(x => Console.WriteLine(x.GetFormattedString()));
            }
            catch (Exception e)
            {
                var message = "Exception thrown while writing Output3";
                WriteExceptionMessage(e, "Output3", message);
            }
        }

        public static void WriteFormattedRecord(Person person)
        {
            Console.WriteLine(GetFormattedRecordString(person));
            //Console.WriteLine("Name: {0,-30} | Gender: {1,-7} | Favorite Color: {2,-15} | DOB: {3,-10}",
            //                        $"{person.LastName}, {person.FirstName}",
            //                        person.Gender,
            //                        person.FavoriteColor,
            //                        person.DateOfBirth.ToString("M/d/yyyy"));
        }

        public static string GetFormattedRecordString(Person person)
        {
            var result = ("Name: {0,-30} | Gender: {1,-7} | Favorite Color: {2,-15} | DOB: {3,-10}",
                                    $"{person.LastName}, {person.FirstName}",
                                    person.Gender,
                                    person.FavoriteColor,
                                    person.DateOfBirth.ToString("M/d/yyyy")).ToString();

            return result;
        }

        public string SafeString(object obj, bool trimString = true)
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

        public Person MakePersonFromStringList(IEnumerable<string> recordStringFields)
        {
            var newPerson = new Person();

            try
            {
                newPerson = new Person
                {
                    LastName = SafeString(recordStringFields.ElementAt(0)),
                    FirstName = SafeString(recordStringFields.ElementAt(1)),
                    Gender = SafeString(recordStringFields.ElementAt(2)),
                    FavoriteColor = SafeString(recordStringFields.ElementAt(3)),
                    DateOfBirth = ParseDateString(recordStringFields.ElementAt(4))
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

        public IEnumerable<string> ReadLinesFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Console.WriteLine("Invalid path");
                return null;
            }

            return File.ReadAllLines(filePath);
        }

        public IEnumerable<string> ReadFileAndSplitByDelim(string path, char[] delim)
        {
            var splitStringObject = new List<string>();
            try
            {
                var allLines = ReadLinesFromFile(path);

                foreach (var recordString in allLines)
                {
                    if (string.IsNullOrEmpty(recordString)) continue;

                    splitStringObject = SplitAndSafeStringLine(recordString).ToList();
                }
            }
            catch (Exception e)
            {
                var message = "Exception thrown while creating record dictionary";
                WriteExceptionMessage(e, "ReadFileAndSplitByDelim", message);
            }

            return splitStringObject;
        }

        public IEnumerable<string> SplitAndSafeStringLine(string inputString)
        {
            var stringPersonObject = inputString.Split('|').ToList();
            stringPersonObject.ForEach(field => { field = SafeString(field); });
            stringPersonObject.RemoveAll(x => x == " " || x == "|" || x == "," || x == string.Empty);
            return stringPersonObject;
        }

        public DateTime ParseDateString(string dateString)
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
}
