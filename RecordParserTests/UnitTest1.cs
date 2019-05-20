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


        [TestMethod]
        public void SafeStringTest()
        {
            var helper = new ParseHelper();
            Assert.AreEqual(helper.SafeString("Foo"), "Foo");
            Assert.AreEqual(helper.SafeString(" Foo   "), "Foo");
            Assert.AreEqual(helper.SafeString(1), "1");
            Assert.AreEqual(helper.SafeString(1.25), "1.25");
            Assert.AreEqual(helper.SafeString("    "), "");
            Assert.AreEqual(helper.SafeString(null), "");
        }

        [TestMethod]
        public void ReadFileAndSplitLinesByDelim_SplitAndSafeStringLine_Pipe()
        {
            var helper = new ParseHelper();
            var expectedOutput = new string[] { "Zebedane", "Zebediah", "M", "Purple", "10/26/1982" };
            var pipeInputString = "Zebedane | Zebediah | M | Purple | 10/26/1982";

            var personDataList = helper.SplitAndSafeStringLine(pipeInputString).ToList();
                                    
            var inc = 0;
            personDataList.ForEach(field =>
            {
                Assert.AreEqual(expectedOutput[inc], field);
                inc++;
            });
        }

        [TestMethod]
        public void ReadFileAndSplitLinesByDelim_SplitAndSafeStringLine_Comma()
        {
            var helper = new ParseHelper();
            var expectedOutput = new string[] { "Zebedane", "Zebediah", "M", "Purple", "10/26/1982" };
            var commaInputString = "Zebedane, Zebediah, M, Purple, 10/26/1982";

            var personDataList = helper.SplitAndSafeStringLine(commaInputString).ToList();

            var inc = 0;

            personDataList.ForEach(field =>
            {
                Assert.AreEqual(expectedOutput[inc], field);
                inc++;
            });
        }

        [TestMethod]
        public void ReadFileAndSplitLinesByDelim_SplitAndSafeStringLine_Space()
        {
            var helper = new ParseHelper();
            var expectedOutput = new string[] { "Zebedane", "Zebediah", "M", "Purple", "10/26/1982" };
            var spaceInputString = "Zebedane Zebediah M Purple 10/26/1982";

            var personDataList = helper.SplitAndSafeStringLine(spaceInputString).ToList();

            var inc = 0;
            personDataList.ForEach(field =>
            {
                Assert.AreEqual(expectedOutput[inc], field);
                inc++;
            });
        }

        [TestMethod]
        public void ParseFieldsIntoPerson()
        {
            var stringObject = new string[] { "Zebedane", "Zebediah", "M", "Purple", "10/26/1982" };
            var helper = new ParseHelper();
            var testPerson = new Person(stringObject);
            var expectedPerson = new Person
            {
                LastName = "Zebedane",
                FirstName = "Zebediah",
                Gender = "M",
                FavoriteColor = "Purple",
                DateOfBirth = new DateTime(1982, 10, 26)
            };
            Assert.AreEqual(expectedPerson.GetFormattedString(), testPerson.GetFormattedString());
        }

        public void Person_ParseDateString()
        {
            var person = new Person();
            var date1 = person.ParseDateString("10/26/1982");
            var date2 = person.ParseDateString("26/10/1982");
            var date3 = person.ParseDateString("2015-05-16T05:50:06");
            var date4 = person.ParseDateString("blahblahblah");

            Assert.AreEqual(new DateTime(1982, 10, 26), date1);
            Assert.AreEqual(new DateTime(1982, 10, 26), date2);
            Assert.AreEqual(new DateTime(1982, 10, 26), date3);

            Assert.AreEqual(DateTime.MinValue, date4);



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
