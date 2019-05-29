using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using ParseHelperLibrary;
using System.IO;

namespace RecordParserTests
{
    [TestClass]
    public class ParserTest
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
                new Record(new string[] {"Smith", "Robert", "M", "Purple", "11/26/1992" }),
                new Record(new string[] {"Zebedane", "Zebediah", "Male", "Purple", "10/26/1982" }),
                new Record(new string[] {"Doe", "Jane", "FEM", "yello", "2/19/1942" })
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
                new Record(new string[] {"Smith", "Robert", "M", "Purple", "11/26/1992" }),
                new Record(new string[] {"Zebedane", "Zebediah", "Male", "Purple", "10/26/1982" }),
                new Record(new string[] {"Doe", "Jane", "FEM", "yello", "2/19/1942" })
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
                new Record(new string[] {"Smith", "Robert", "M", "Purple", "11/26/1992" }),
                new Record(new string[] {"Zebedane", "Zebediah", "Male", "Purple", "10/26/1982" }),
                new Record(new string[] {"Doe", "Jane", "FEM", "yello", "2/19/1942" })
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

        [TestMethod]
        public void ParseTextFilesTest()
        {
            var helper = new ParseHelper();
            var currentDirectory = Directory.GetCurrentDirectory();
            var filePathCollection = new string[] 
            {
                Path.Combine(currentDirectory,"RecordFile1.txt"),
                Path.Combine(currentDirectory,"RecordFile2.txt"),
                Path.Combine(currentDirectory,"RecordFile3.txt")
            };

            var listOfAllRecordStringCollections = helper.ReadFileAndSplitLines(filePathCollection);

            Assert.IsNotNull(listOfAllRecordStringCollections);
            Assert.AreEqual(15, listOfAllRecordStringCollections.Count());
            foreach (IEnumerable<string> stringCollection in listOfAllRecordStringCollections)
            {
                Assert.IsNotNull(stringCollection);
                Assert.IsFalse(stringCollection.Any(sc => string.IsNullOrWhiteSpace(sc)));
                Assert.AreEqual(5, stringCollection.Count());
            }
        }
    }
}
