using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordParserTests;
using RecordParser;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecordParserTests
{
    [TestClass]
    public class ParserTest
    {
        //[TestMethod]
        //public void MainTest()
        //{
        //    var objectList = new List<Record>();
        //    var helper = new ParseHelper();
        //    var delimArray = new char[] { '|', ',', ' ' };
        //    var filePath = "D:\\Users\\john.schwartz\\source\\repos\\RecordParser\\RecordParser\\RecordFile1.txt";
        //    //var listOfInputStrings = new List<string> {
        //    //    "Zebedane | Zebediah | M | Purple | 10/26/1982",
        //    //    "Zebedane, Zebediah, M, Purple, 10/26/1982",
        //    //    "Zebedane Zebediah M Purple 10/26/1982"
        //    //};
        //    Console.WriteLine("Start");

        //    // Read each line of the the file, split the data by the delimiters, return the cleaned up string array
        //    var recordList = helper.ReadFileAndSplitLines(filePath, delimArray).ToList();
        //    // for each record data string array, add it to the list
        //    recordList.ForEach(strArray => objectList.Add(new Record(strArray)));


        //}



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

        //[TestMethod]
        //public void ReadFileAndSplitLines_Test()
        //{
        //    var helper = new ParseHelper();
        //    //var filePath = "\\\\abqdatw01\\users\\john.schwartz\\Desktop\\RecordParser\\RecordParser\\RecordParser\\UnitTestRecordFile.txt";

        //    Assert.IsTrue(File.Exists(filePath));

        //    var expectedResult = new List<List<string>>
        //            {
        //                new List<string> { "Downy", "Robert", "M", "Purple", "11/26/1992"},
        //                new List<string> { "Zebedane", "Zebediah", "M", "Purple", "10/26/1982"},
        //                new List<string> { "Does", "Jane", "FEM", "yello", "2/19/1942" }
        //            };

        //    var listOfObjectDataLists = helper.ReadFileAndSplitLines(filePath);
        //    var listOfOutputRecords = new List<Record>();
        //    Assert.IsNotNull(listOfObjectDataLists);

        //    for (var o = 0; o < 3; o++)
        //    {
        //        var expectedList = expectedResult[o];
        //        var testList = listOfObjectDataLists.ElementAt(o).ToList();
        //        for (var i = 0; i < 5; i++)
        //        {
        //            Assert.IsNotNull(expectedList[i]);
        //            Assert.IsNotNull(testList[i]);
        //            Assert.AreEqual(expectedList[i], testList[i]);
        //        }
        //    }

        //    //expectedResult.ForEach(res => )
        //}

        [TestMethod]
        public void NoArgumentsTest()
        {
            var helper = new ParseHelper();
            var recordList = new List<Record>();

            var test = helper.ReadFileAndSplitLines(new string[] { }).ToList(); // run with no input files
            test.ForEach(strArray => recordList.Add(new Record(strArray)));

            var birthdateOut = helper.GetByBirthdate(recordList);
            
            Assert.IsNotNull(test);
            Assert.AreEqual(0, birthdateOut.Count());
        }

        [TestMethod]
        public void InvalidArgumentsTest()
        {
            var helper = new ParseHelper();
            var recordList = new List<Record>();

            var test = helper.ReadFileAndSplitLines(new string[] { "InvalidFile1.txt", "InvalidFile2.txt", "InvalidFile3.txt", }).ToList(); // run with no input files
            test.ForEach(strArray => recordList.Add(new Record(strArray)));

            var birthdateOut = helper.GetByBirthdate(recordList);
                        
            Assert.IsNotNull(birthdateOut);
            Assert.AreEqual(0, birthdateOut.Count());
        }

        [TestMethod]
        public void GenderSortTest()
        {
            var helper = new ParseHelper();

            var recordList = new List<Record>
            {
                new Record("Smith", "Robert", "M", "Purple", "11/26/1992"),
                new Record("Zebedane", "Zebediah", "Male", "Purple", "10/26/1982"),
                new Record("Doe", "Jane", "FEM", "yello", "2/19/1942" ),
            };

            var genderResult = helper.GetByGender(recordList);
            Assert.IsNotNull(genderResult);
            Assert.AreNotEqual(0, genderResult.Count());
            Assert.AreEqual(3, genderResult.Count());
            Assert.IsTrue(genderResult.First().Gender[0] == 'f' || genderResult.First().Gender[0] == 'F');
            Assert.IsTrue(genderResult.Last().Gender[0] == 'm' || genderResult.Last().Gender[0] == 'M');
        }

        [TestMethod]
        public void BirthdateSortTest()
        {
            var helper = new ParseHelper();

            var recordList = new List<Record>
            {
                new Record("Smith", "Robert", "M", "Purple", "11/26/1992"),
                new Record("Zebedane", "Zebediah", "Male", "Purple", "10/26/1982"),
                new Record("Doe", "Jane", "FEM", "yello", "2/19/1942" ),
            };

            var birthdateResult = helper.GetByBirthdate(recordList);
            Assert.IsNotNull(birthdateResult);
            Assert.AreNotEqual(0, birthdateResult.Count());
            Assert.AreEqual(3, birthdateResult.Count());
            Assert.AreEqual(DateTime.Parse("2/19/1942"), birthdateResult.First().DateOfBirth);
            Assert.AreEqual(DateTime.Parse("11/26/1992"), birthdateResult.Last().DateOfBirth);            
        }

        [TestMethod]
        public void lastNameSortTest()
        {
            var helper = new ParseHelper();

            var recordList = new List<Record>
            {
                new Record("Smith", "Robert", "M", "Purple", "11/26/1992"),
                new Record("Zebedane", "Zebediah", "Male", "Purple", "10/26/1982"),
                new Record("Doe", "Jane", "FEM", "yello", "2/19/1942" ),
            };

            var nameResult = helper.GetByLastname(recordList);
            Assert.IsNotNull(nameResult);
            Assert.AreNotEqual(0, nameResult.Count());
            Assert.AreEqual(3, nameResult.Count());
            Assert.AreEqual("Zebedane", nameResult.First().LastName);
            Assert.AreEqual("Doe", nameResult.Last().LastName);
        }

        [TestMethod]
        public void SplitAndSafeStringLine_Pipe()
        {
            var helper = new ParseHelper();
            var expectedOutput = new string[] { "Zebedane", "Zebediah", "M", "Purple", "10/26/1982" };
            var pipeInputString = "Zebedane | Zebediah | M | Purple | 10/26/1982";

            var RecordDataList = helper.SplitAndSafeStringLine(pipeInputString).ToList();
            Assert.IsNotNull(RecordDataList);

            var inc = 0;
            RecordDataList.ForEach(field =>
            {
                Assert.AreEqual(expectedOutput[inc], field);
                inc++;
            });
        }

        [TestMethod]
        public void SplitAndSafeStringLine_Comma()
        {
            var helper = new ParseHelper();
            var expectedOutput = new string[] { "Zebedane", "Zebediah", "M", "Purple", "10/26/1982" };
            var commaInputString = "Zebedane, Zebediah, M, Purple, 10/26/1982";

            var RecordDataList = (helper.SplitAndSafeStringLine(commaInputString) ?? new string[0]).ToList();

            var inc = 0;

            RecordDataList.ForEach(field =>
            {
                Assert.AreEqual(expectedOutput[inc], field);
                inc++;
            });
        }

        [TestMethod]
        public void SplitAndSafeStringLine_Space()
        {
            var helper = new ParseHelper();
            var expectedOutput = new string[] { "Zebedane", "Zebediah", "M", "Purple", "10/26/1982" };
            var spaceInputString = "Zebedane Zebediah M Purple 10/26/1982";

            var RecordDataList = helper.SplitAndSafeStringLine(spaceInputString).ToList();

            var inc = 0;
            RecordDataList.ForEach(field =>
            {
                Assert.IsNotNull(field);
                Assert.AreEqual(expectedOutput[inc], field);
                inc++;
            });
        }
        
        [TestMethod]
        public void ParseFieldsIntoRecord()
        {
            var stringObject = new string[] { "Zebedane", "Zebediah", "M", "Purple", "10/26/1982" };
            var helper = new ParseHelper();
            var testRecord = new Record(stringObject);
            var expectedRecord = new Record
            {
                LastName = "Zebedane",
                FirstName = "Zebediah",
                Gender = "M",
                FavoriteColor = "Purple",
                DateOfBirth = new DateTime(1982, 10, 26)
            };
            Assert.AreEqual(expectedRecord.ToString(), testRecord.ToString());
        }

        

        //[TestMethod]
        //public void Record_ParseDateString()
        //{
        //    var Record = new Record();
        //    var date1 = Record.ParseDateString("10/26/1982");
        //    var date2 = Record.ParseDateString("blahblahblah");
        //    var date3 = Record.ParseDateString("1982-10-26T05:50:00");


        //    Assert.AreEqual(new DateTime(1982, 10, 26), date1);
        //    Assert.AreEqual(DateTime.MinValue, date2);
        //    Assert.AreEqual(new DateTime(1982, 10, 26, 5, 50, 0), date3);
        //}






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
        // Can remove space, pipe, comma indiv values from 
    }
}
