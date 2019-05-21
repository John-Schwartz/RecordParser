using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordParser
{
    public class ParseHelper
    {
        // Takes any object and safely executes ToString, with optional string trimming. Returns an empty string if any part failed.
        public string SafeString(object obj, bool trimString = true)
        {
            if (trimString) return obj?.ToString().Trim() ?? string.Empty;
            return obj?.ToString() ?? string.Empty;
        }

        public IEnumerable<IEnumerable<string>> ReadFileAndSplitLines(string filePath, char[] delims)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Console.WriteLine("Invalid path");
                return null;
            }
            
            var returnList = new List<IEnumerable<string>>();
            try
            {
                var allLines = File.ReadAllLines(filePath);
                
                // For each line in the file, if it isn't empty, split by the delimiter and trim space.
                // Once split and cleaned up, add the string array to the return list
                foreach (var recordLine in allLines)
                {
                    if (string.IsNullOrEmpty(recordLine)) continue;                                        
                    returnList.Add(SplitAndSafeStringLine(recordLine));
                }
            }
            catch (Exception e)
            {
                var message = "Exception thrown while creating record dictionary";
                WriteExceptionMessage(e, "ReadFileAndSplitByDelim", message);
            }

            return returnList;
        }

        // Split the input string by the delimiters, then run safestring on each split string item
        // Remove any whitespace, remaining delims or empty elements, then return the string array
        public IEnumerable<string> SplitAndSafeStringLine(string inputString)
        {            
            var stringPersonObject = inputString?.Split('|', ',', ' ').ToList() ?? new List<string>();
            stringPersonObject.ForEach(field => { field = SafeString(field); });
            stringPersonObject.RemoveAll(x => x == " " || x == "|" || x == "," || x == string.Empty);
            return stringPersonObject;
        }

        public static void WriteExceptionMessage(Exception e, string functionName = "", string humanMessage = "")
        {
            Console.WriteLine($"{functionName} - {humanMessage}\n");
            Console.WriteLine(e.Message);
        }
    }
}
