
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

        // Query style linq for sorting by requirements: gender, dob, lastname
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
                
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) continue; // If the filepath is invalid or empty, skip it

                // using the filepath, read the file into an array of strings (one line per string)
                string[] allLines = File.ReadAllLines(filePath); 

                // For each line in the file
                foreach (string recordLine in allLines)
                {                    
                    if (string.IsNullOrEmpty(recordLine)) continue;         // if the line isn't empty or null,
                    var stringArray = SplitAndSafeStringLine(recordLine);   // split by the delimiter(s) and trim white space.
                    if (stringArray != null) returnList.Add(stringArray);   // then add the string array to the return list
                }
            }

            return returnList;
        }

        // Overload to add functionality for individual files. Simply passes single-item enumerable
        public IEnumerable<IEnumerable<string>> ReadFileAndSplitLines(string filePath) => ReadFileAndSplitLines(new string[] { filePath });

        // Checks the validity of the string array for making a Record. 
        // Date must parse, gender must begin with f or m, and requires 5 data fields
        public bool StringCollectionIsValid(IEnumerable<string> stringArray)
        {
            if (stringArray == null) throw new ArgumentNullException();
            
            var stringCollection = stringArray.ToList();
            if (stringCollection.Count() < 5                                            // Must have 5 values to match Record object's properties
                || !stringCollection.TrueForAll(str => !string.IsNullOrWhiteSpace(str)) // No whitespace or null values
                || ParseDateString(stringCollection.ElementAt(4)) == DateTime.MinValue  // If result of TryParse returns Min, it failed
                ) return false;

            // If the gender starts with f or m, case independent, the collection is now valid.
            // TODO: I believe the regex is off? verify, because it breaks here!
            if (Regex.IsMatch(@"?is^(m|f)", stringCollection[2])) return true; 
            
            // Otherwise, collection is invalid
            return false;
        }

        readonly List<char> delimiters = new List<char> { ' ', ',', '|' };

        // Split the input string by the delimiters, then run safestring on each split string item
        // Remove any empty string, remaining delims or whitespace elements, then return the string array
        public IEnumerable<string> SplitAndSafeStringLine(string inputString)
        {
            var stringRecordObject = inputString.Split(new char[] { ' ', ',', '|' }, StringSplitOptions.None).ToList();
            var results = stringRecordObject.Select(str => SafeString(str)).Where(str => str != " " || str != "|" || str != "," || !string.IsNullOrWhiteSpace(str));
            if (!StringCollectionIsValid(results)) return new string[0];
            return results;
        }
        //readonly List<char> delimiters = new List<char> { ' ', ',', '|' };
        //internal IEnumerable<string> SplitAndSafeStringLine(string inputString)
        //{// NOTE: Can't be internal if used by webAPI controller
        //    var stringRecordObject = inputString.Split(delimiters.ToArray(), StringSplitOptions.None).ToList();
        //    //stringRecordObject.ForEach(field => { field = SafeString(field); });
        //    //stringRecordObject.RemoveAll(x => x == " " || x == "|" || x == "," || string.IsNullOrWhiteSpace(x));

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
