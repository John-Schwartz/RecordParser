using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordParser
{
    public class ParseHelper
    {
        public void PrintResults(List<Record> RecordList)
        {
            Console.WriteLine("\n\nSorted by Gender, then last name descending");
            GetByGender(RecordList).ForEach(x => Console.WriteLine(x.ToString()));

            Console.WriteLine("\n\nSorted by Date Of Birth");
            GetByBirthdate(RecordList).ForEach(x => Console.WriteLine(x.ToString()));

            Console.WriteLine("\n\nSorted by Last Name");
            GetByLastname(RecordList).ForEach(x => Console.WriteLine(x.ToString()));
        }

        public List<Record> GetByGender(IEnumerable<Record> RecordList) => (from p in RecordList
                                                                            orderby p.Gender, p.LastName ascending
                                                                            select p).ToList();
        public List<Record> GetByBirthdate(IEnumerable<Record> RecordList) => (from p in RecordList
                                                                               orderby p.DateOfBirth ascending
                                                                               select p).ToList();
        public List<Record> GetByLastname(IEnumerable<Record> RecordList) => (from p in RecordList
                                                                              orderby p.LastName descending
                                                                              select p).ToList();

        // Takes any object and safely executes ToString, with optional string trimming. Returns an empty string if any part failed.
        public string SafeString(object obj, bool trimString = true)
        {
            if (trimString) return obj?.ToString().Trim() ?? string.Empty;
            return obj?.ToString() ?? string.Empty;
        }

        public IEnumerable<IEnumerable<string>> ReadFileAndSplitLines(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return null;

            var returnList = new List<IEnumerable<string>>();
            try
            {
                var allLines = File.ReadAllLines(filePath);

                // For each line in the file, if it isn't empty, split by the delimiter and trim space.
                // Once split and cleaned up, add the string array to the return list
                foreach (var recordLine in allLines)
                {
                    if (string.IsNullOrEmpty(recordLine)) continue;
                    var stringArray = SplitAndSafeStringLine(recordLine);
                    if (StringArrayIsValid(stringArray)) returnList.Add(stringArray);
                }
            }
            catch (Exception e)
            {
                var message = "Exception thrown while creating record dictionary";
                WriteExceptionMessage(e, "ReadFileAndSplitByDelim", message);
            }

            return returnList;
        }

        public bool StringArrayIsValid(IEnumerable<string> stringArray)
        {
            if (stringArray == null
                || !stringArray.Any()
                || stringArray.Count() < 5
                || !stringArray.ToList().TrueForAll(str => !string.IsNullOrEmpty(str))
                || ParseDateString(stringArray.ElementAt(4)) == DateTime.MinValue
                ) return false;
            var genderValue = stringArray.ElementAt(2);
            if (genderValue.StartsWith("m", true, CultureInfo.CurrentCulture)
                || genderValue.StartsWith("f", true, CultureInfo.CurrentCulture)
                //|| genderValue.StartsWith("M", true, CultureInfo.CurrentCulture)
                //|| genderValue.StartsWith("F", true, CultureInfo.CurrentCulture)
                ) return false;

            return true;
        }

        // Split the input string by the delimiters, then run safestring on each split string item
        // Remove any empty string, remaining delims or whitespace elements, then return the string array
        public IEnumerable<string> SplitAndSafeStringLine(string inputString)
        {
            var stringRecordObject = inputString?.Split(new char[] { '|', ',', ' ' }, StringSplitOptions.None).ToList() ?? new List<string>();
            stringRecordObject.ForEach(field => { field = SafeString(field); });
            stringRecordObject.RemoveAll(x => x == " " || x == "|" || x == "," || string.IsNullOrEmpty(x));
            return stringRecordObject;
        }

        public static void WriteExceptionMessage(Exception e, string functionName = "", string humanMessage = "")
        {
            Console.WriteLine($"{functionName} - {humanMessage}\n");
            Console.WriteLine(e.Message);
        }

        private DateTime ParseDateString(string dateString)
        {
            DateTime.TryParse(dateString, out DateTime result);
            if (result == null || result == DateTime.MinValue)
            {
                return DateTime.MinValue;
            }
            return result;
        }
    }
}
