
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ParseHelperLibrary
{
    public class ParseHelper
    {
        // Writes each record to the console as a formatted string.
        public void PrintResults(List<Record> RecordList)
        {
            Console.WriteLine("\n\nSorted by Gender, then last name descending");
            GetByGender(RecordList).ForEach(x => Console.WriteLine(x.ToString()));

            Console.WriteLine("\n\nSorted by Date Of Birth");
            GetByBirthdate(RecordList).ForEach(x => Console.WriteLine(x.ToString()));

            Console.WriteLine("\n\nSorted by Last Name");
            GetByLastname(RecordList).ForEach(x => Console.WriteLine(x.ToString()));
        }

        // Query-style linq for sorting by requirements: gender+lastname, dob, lastname
        public List<Record> GetByGender(IEnumerable<Record> RecordList) => (from p in RecordList
                                                                            orderby p.Gender, p.LastName ascending
                                                                            select p).ToList();
        public List<Record> GetByBirthdate(IEnumerable<Record> RecordList) => (from p in RecordList
                                                                               orderby p.DateOfBirth ascending
                                                                               select p).ToList();
        public List<Record> GetByLastname(IEnumerable<Record> RecordList) => (from p in RecordList
                                                                              orderby p.LastName descending
                                                                              select p).ToList();

        // Takes any object and safely executes ToString, with optional string trimming. 
        // Returns an empty string if any part fails.
        public string SafeString(object obj, bool trimString = true)
        {
            if (trimString) return obj?.ToString().Trim() ?? string.Empty;
            return obj?.ToString() ?? string.Empty;
        }

        // Takes an enumerable of file paths, parses all of the lines and returns a consolidated array
        public IEnumerable<IEnumerable<string>> ReadFileAndSplitLines(IEnumerable<string> filePaths)
        {
            var returnList = new List<IEnumerable<string>>();
            foreach (string filePath in filePaths)
            {                
                if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath)) continue; // If the filepath is invalid or empty, skip it  
                
                string[] allLines = File.ReadAllLines(filePath); // using the filepath, read the file into an array of strings (one line per string)
                                
                foreach (string recordLine in allLines)                     
                {                                                                               // For each line in the file
                    if (string.IsNullOrWhiteSpace(recordLine)) continue;                        // if the line isn't empty or null,
                    IEnumerable<string> stringCollection = SplitAndSafeStringLine(recordLine);  // split by the delimiter(s) and trim white space.
                    if (stringCollection != null) returnList.Add(stringCollection.ToList());    // then add the string array to the return list
                }
            }
            return returnList;
        }
        // Overload for individual files. Simply passes single-item enumerable
        public IEnumerable<IEnumerable<string>> ReadFileAndSplitLines(string filePath) => ReadFileAndSplitLines(new string[] { filePath });

        // Checks the validity of the string array for making a Record.
        public bool StringCollectionIsValid(IEnumerable<string> stringArray)
        {
            if (stringArray == null) throw new ArgumentNullException();        // If the array is null, something has gone terribly wrong
            
            var stringCollection = stringArray.ToList();
            if (stringCollection.Count() < 5                                   // Must have 5 values to match Record object's properties
                || stringCollection.Any(str => string.IsNullOrWhiteSpace(str)) // No whitespace or null values
                || ParseDateString(stringCollection[4]) == DateTime.MinValue   // Date must be parseable If result of TryParse returns Min, it failed
                || !Regex.IsMatch(stringCollection[2], @"(?i)^(m|f)")          // gender field must start with f,F,m,M
                ) return false;
                        
            return true;
        }
        
        // Split the input string by the delimiters, tossing empty values, then safestring each split string item
        // finally, return the string array
        public IEnumerable<string> SplitAndSafeStringLine(string inputString)
        {
            var stringRecordObject = inputString.Split(new char[] { ' ', ',', '|' }, StringSplitOptions.RemoveEmptyEntries).Select(str => SafeString(str)).ToList();
            if (!StringCollectionIsValid(stringRecordObject)) return new string[0];
            return stringRecordObject;
        }
        //readonly List<char> delimiters = new List<char> { ' ', ',', '|' };
        //internal IEnumerable<string> SplitAndSafeStringLine(string inputString)
        //{// NOTE: Can't be internal if used by webAPI controller
        //    var stringRecordObject = inputString.Split(delimiters.ToArray(), StringSplitOptions.None).ToList();
        //    //stringRecordObject.ForEach(field => { field = SafeString(field); });
        //    //stringRecordObject.RemoveAll(x => x == " " || x == "|" || x == "," || string.IsNullOrWhiteSpace(x));
        //stringRecordObject.RemoveAll(x => x == " " || x == "|" || x == "," || string.IsNullOrWhiteSpace(x));
        //var results = stringRecordObject.Where(str => str != " " || str != "|" || str != "," || !string.IsNullOrEmpty(str));

        //    // Can't get List/array of char/string to work nicely and legibly. While not quite as concise as the attempt w/ readonly delim array, I believe this is legible and only slightly longer.
        //    //var results = stringRecordObject.Where(s => !delimiters.ToList().Contains(s)).Select(s => SafeString(s));
        //    var testResults = stringRecordObject.Select(str => SafeString(str)).Where(str => str != " " || str != "|" || str != "," || !string.IsNullOrWhiteSpace(str));

        //    if (!StringCollectionIsValid(stringRecordObject)) return new string[0];
        //    return stringRecordObject;
        //}

        private DateTime ParseDateString(string dateString)
        {
            DateTime.TryParse(dateString, out DateTime result);
            return result;
        }
    }


}
