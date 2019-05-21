﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordParser
{
    public class ParseHelper
    {
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

        public IEnumerable<IEnumerable<string>> ReadFileAndSplitLines(string filePath, char[] delims)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Console.WriteLine("Invalid path");
                return null;
            }


            var splitStringObject = new List<string>();
            var returnList = new List<IEnumerable<string>>();
            try
            {
                var allLines = File.ReadAllLines(filePath); 

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

        public IEnumerable<string> SplitAndSafeStringLine(string inputString)
        {
            
            var stringPersonObject = inputString.Split('|',',',' ').ToList();
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
