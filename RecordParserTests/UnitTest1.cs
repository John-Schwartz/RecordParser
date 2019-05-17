using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESTfulAPI;
using RecordParserTests;
using RecordParser;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecordParserTests
{
    [TestClass]
    public class UnitTest1
    {


        // Given a file path, read the lines, and parse each line by delimiter
        public void ReadFileAndSplitByDelim_HappyPath()
        {
            var helper = new ParseHelper();
            var delimArray = new char[] { '|', ',', ' ' };

            var filePath = "D:\\Users\\john.schwartz\\source\\repos\\RecordParser\\RecordParser\\RecordFilePipe.txt";
            //Assert.IsTrue(File.Exists(filePath));
                                   
            var result = helper.ReadFileAndSplitByDelim(filePath, delimArray);
            Assert.Equals(result, "Name: Downy, Robert | Gender: M | Favorite Color: Purple | DOB: 11 / 26 / 1992");
        }

        [TestMethod]
        public void ReadFileAndSplitByDelim_SplitAndSafeStringLine()
        {
            var helper = new ParseHelper();
            var expectedOutput = new string[] { "Zebedane", "Zebediah", "M", "Purple", "10/26/1982" };
            var inputString = "Zebedane | Zebediah | M | Purple | 10/26/1982";
            var stringPersonObject = inputString.Split('|');

            var inc = 0;

            stringPersonObject.ToList().ForEach(field =>
            {
                field = helper.SafeString(field);
                Assert.AreEqual(expectedOutput[inc], field);
                inc++;
            });

            //foreach (var field in stringPersonObject)
            //{
            //    stringPersonObject[inc] = helper.SafeString(field);
            //    Assert.AreEqual(expectedOutput[inc], field);
            //    inc++;
            //}

        }

        [TestMethod]
        public void ReadFileAndSplitByDelim_ParseFieldsInStringObject()
        {
            

        }


        //public string pipeTestData = " Lname | Fname | Gender | FColor | 8/25/1997 \n Emanl | Emanf | Redneg | Rolocf | 7/15/1989 ";
        //public string commaTestData = "";
        //public string spaceTestData = "";
        //public string TestFilePath = "D:\\Users\\john.schwartz\\source\\repos\\RecordParser\\RecordParser\\RecordFile1.txt";

        //[TestMethod]
        //public void ReadFileLinesAtPath_FileExists()
        //{
        //    var helper = new ParseHelper();

        //    Assert.IsTrue(File.Exists(TestFilePath));
        //}

        //[TestMethod]
        //public void ReadFileLinesAtPath_StringEnumIsValid()
        //{
        //    var helper = new ParseHelper();

        //    Assert.IsNotNull(helper.ReadFileLinesAtPath(TestFilePath));
        //}


        //[TestMethod]
        //public void SafeString_NullSafe()
        //{            
        //    var result = RecordParser.Program.SafeString(null);

        //    Assert.AreEqual(result, string.Empty);
        //}

        //[TestMethod]
        //public void SafeString_TrimString()
        //{
        //    var testString = "   trimmed      ";
        //    var result = RecordParser.Program.SafeString(testString);
        //    Console.WriteLine(testString);
        //    //Assert.AreEqual(result, "trimmed");
        //}

        // Can parse pipes and split to 5
        // Can parse commas and split to 5
        // Can parse spaces
        // Can parse dates of multiple formats
        // Can remove space, pipe, comma indiv values from 

        public void ReadFileAndSplitByDelim_SplitRow_SplitByPipe()
        {

        }

        public void ReadFileAndSplitByDelim_SplitRow_SplitByComma()
        {

        }

        public void ReadFileAndSplitByDelim_SplitRow_SplitBySpace()
        {

        }


        public void Output1()
        {

        }
    }
}
