
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ParseHelperLibrary
{
    /// <summary>
    /// ParseHelper is a helper class to perform IO functions on records
    /// </summary>
    public class ParseHelper
    {

        /// <summary>
        /// Writes each record to the console as a formatted string, sorted as specified
        /// </summary>
        /// <param name="recordList">The List which should contain the parsed records for display</param>
        /// <returns></returns>
        public void PrintResults(IEnumerable<Record> recordList)
        {
            Console.WriteLine("\n\nSorted by Gender, then last name descending");
            GetByGender(recordList).ForEach(x => Console.WriteLine(x.ToString()));

            Console.WriteLine("\n\nSorted by Date Of Birth");
            GetByBirthdate(recordList).ForEach(x => Console.WriteLine(x.ToString()));

            Console.WriteLine("\n\nSorted by Last Name");
            GetByLastname(recordList).ForEach(x => Console.WriteLine(x.ToString()));
        }

        /// <summary>
        /// Sorts an enumerable of records by gender (females before males) then by last name ascending
        /// </summary>
        /// <param name="recordList">the enumerable of Record objects that should be sorted</param>
        /// <returns>List of Records</returns>
        public List<Record> GetByGender(IEnumerable<Record> recordList) => (from p in recordList
                                                                            orderby p.Gender, p.LastName ascending
                                                                            select p).ToList();
        /// <summary>
        /// Sorts an enumerable of records by birth date ascending
        /// </summary>
        /// <param name="recordList"> the enumerable of Record objects that should be sorted</param>
        /// <returns>List of Records</returns>
        public List<Record> GetByBirthdate(IEnumerable<Record> recordList) => (from p in recordList
                                                                               orderby p.DateOfBirth ascending
                                                                               select p).ToList();
        /// <summary>
        /// Sorts an enumerable of records by last name descending
        /// </summary>
        /// <param name="recordList">the enumerable of Record objects that should be sorted</param>
        /// <returns>List of Records</returns>
        public List<Record> GetByLastname(IEnumerable<Record> recordList) => (from p in recordList
                                                                              orderby p.LastName descending
                                                                              select p).ToList();

        // Takes any object and safely executes ToString, with optional string trimming. 
        // Returns an empty string if any part fails.
        /// <summary>
        /// Takes any object and safely execute ToString, with optional string trimming.
        /// </summary>
        /// <param name="obj">The object being converted</param>
        /// <param name="trimString">bool. Default true</param>
        /// <returns>The object as string, or an empty string</returns>
        public string SafeString(object obj, bool trimString = true)
        {
            if (trimString)
            {
                return obj?.ToString().Trim() ?? string.Empty;
            }
            return obj?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Takes an enumerable of file paths, reads and parses all of the lines into enumerables
        /// of record data as strings, then aggregates the record string collections into a 
        /// single list ("jagged list," list of enumerables)
        /// </summary>
        /// <param name="filePaths">enumerable of file paths for parsing.</param>
        /// <returns>A list of string enumerables</returns>
        public IEnumerable<IEnumerable<string>> ReadFileAndSplitLines(IEnumerable<string> filePaths)
        {
            // Take the file paths and create a list of string collections in which to aggregate successfully split lines. 
            return filePaths.Aggregate(new List<IEnumerable<string>>(), (rList, fp) =>
            {
                if (string.IsNullOrWhiteSpace(fp) || !File.Exists(fp))
                {
                    return new List<IEnumerable<string>>();
                }
                // if the file path is valid, read all the lines and split them into a collection of 
                // string values, adding them to the aggregate list
                return File.ReadAllLines(fp).Aggregate(rList, (list, recordLine) =>
                    {
                        if (!string.IsNullOrWhiteSpace(recordLine))
                        {
                            list.Add(SplitAndSafeStringLine(recordLine));
                        }
                        return list;
                    });
            });
        }

        /// <summary>
        /// Overload for individual files. Passes a single-item enumerable to ReadFileAndSplitLines
        /// </summary>
        /// <param name="filePaths">single file path</param>
        /// <returns>A list of string enumerables</returns>
        public IEnumerable<IEnumerable<string>> ReadFileAndSplitLines(string filePath) => ReadFileAndSplitLines(new string[] { filePath });

        /// <summary>
        /// Validates the string array for making a Record. The enumerable must have 5 values to match the
        /// Record object's properties, none of the values may be null or whitespace, the date must be 
        /// parseable, and the gender field must begin with M/m or F/f
        /// </summary>
        /// <param name="stringArray">Enumerable of record string data</param>
        /// <returns>True if the enumerable is valid</returns> 
        public bool StringCollectionIsValid(IEnumerable<string> stringArray)
        {
            var stringCollection = stringArray.ToList();
            if (stringCollection.Count() < 5
                || stringCollection.Any(str => string.IsNullOrWhiteSpace(str))
                || ParseDateString(stringCollection[4]) == DateTime.MinValue
                || !Regex.IsMatch(stringCollection[2], @"(?i)^(m|f)"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Splits the input string by space, comma, and pipe, removing empty entries, then make sure it's trimmed and safe.
        /// Validates the resulting array to ensure successful record creation.
        /// </summary>
        /// <param name="inputString">the string of text to be split</param>
        /// <returns>An enumerable of record string data</returns>
        public IEnumerable<string> SplitAndSafeStringLine(string inputString)
        {
            var stringRecordObject = inputString.Split(new char[] { ' ', ',', '|' }, StringSplitOptions.RemoveEmptyEntries).Select(str => SafeString(str));
            if (!StringCollectionIsValid(stringRecordObject)) return new string[0];
            return stringRecordObject;
        }

        private DateTime ParseDateString(string dateString)
        {
            DateTime.TryParse(dateString, out DateTime result);
            return result;
        }
    }


}
