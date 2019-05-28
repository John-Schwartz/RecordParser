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

        // Takes an enumerable of file paths, parses all of the lines and returns a consolidated array
        public IEnumerable<IEnumerable<string>> ReadFileAndSplitLines(IEnumerable<string> filePaths)
        {
            var returnList = new List<IEnumerable<string>>();
            foreach (var filePath in filePaths)
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) continue;

                var allLines = File.ReadAllLines(filePath);

                // For each line in the file, if it isn't empty, split by the delimiter and trim space.
                // Once split and cleaned up, add the string array to the return list
                foreach (var recordLine in allLines)
                {
                    if (string.IsNullOrEmpty(recordLine)) continue;
                    var stringArray = SplitAndSafeStringLine(recordLine);
                    if (stringArray != null) returnList.Add(stringArray);
                }
            }

            return returnList;
        }

        // Takes a single filepath, parses lines, returns array
        public IEnumerable<IEnumerable<string>> ReadFileAndSplitLines(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return new List<List<string>>();

            var returnList = new List<IEnumerable<string>>();

            var allLines = File.ReadAllLines(filePath);

            // For each line in the file, if it isn't empty, split by the delimiter and trim space.
            // Once split and cleaned up, add the string array to the return list
            foreach (var recordLine in allLines)
            {
                if (string.IsNullOrEmpty(recordLine)) continue;
                var stringArray = SplitAndSafeStringLine(recordLine);
                if (stringArray != null) returnList.Add(stringArray);
            }


            return returnList;
        }

        // Checks the validity of the string array for making a Record. 
        // Date must parse, gender must begin with f or m, and requires 5 data fields
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
                || genderValue.StartsWith("M", true, CultureInfo.CurrentCulture)
                || genderValue.StartsWith("F", true, CultureInfo.CurrentCulture)
                ) return true;

            return false;
        }

        // Split the input string by the delimiters, then run safestring on each split string item
        // Remove any empty string, remaining delims or whitespace elements, then return the string array
        public IEnumerable<string> SplitAndSafeStringLine(string inputString)
        {
            var stringRecordObject = inputString?.Split(new char[] { '|', ',', ' ' }, StringSplitOptions.None).ToList() ?? new List<string>();
            stringRecordObject.ForEach(field => { field = SafeString(field); });
            stringRecordObject.RemoveAll(x => x == " " || x == "|" || x == "," || string.IsNullOrEmpty(x));
            if (!StringArrayIsValid(stringRecordObject)) return new string[0];
            return stringRecordObject;
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
