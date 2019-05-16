using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESTfulAPI;
using RecordParserTests;
using RecordParser;
using System.Collections.Generic;

namespace RecordParserTests
{
    [TestClass]
    public class UnitTest1
    {
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

        public void ReadFileAndSplitByDelim_ReadFile_InvalidPath()
        {

        }

        public void ReadFileAndSplitByDelim_ReadFile_ValidPath()
        {

        }

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
