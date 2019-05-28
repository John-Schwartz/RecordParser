using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESTfulAPI.Controllers;
using Newtonsoft.Json;
using ParseHelperLibrary;

namespace UnitTestProject1
{
    [TestClass]
    public class RecordsControllerTest
    {
        private ParseHelper Helper = new ParseHelper();


        [TestMethod]
        public void BirthdateTest()
        {
            // Arrange
            RecordsController controller = new RecordsController();

            // Act
            PostValid(ref controller); // insert valid results (6 records)
            PostInvalid(ref controller); // attempt to insert invalid results
            
            var resultBirthdateJSON = controller.Get("birthdate");
            var resultBirthdate = JsonConvert.DeserializeObject<List<Record>>(resultBirthdateJSON);

            //Assert
            Assert.IsNotNull(resultBirthdate);
            Assert.AreNotEqual(0, resultBirthdate.Count());
            Assert.AreEqual(6, resultBirthdate.Count());
            Assert.AreEqual(DateTime.Parse("2/19/1942"), resultBirthdate.First().DateOfBirth);
            Assert.AreEqual(DateTime.Parse("12/1/2001"), resultBirthdate.Last().DateOfBirth);
        }

        [TestMethod]
        public void LastNameTest()
        {
            // Arrange
            RecordsController controller = new RecordsController();

            // Act
            PostValid(ref controller); // insert valid results (6 records)
            PostInvalid(ref controller); // attempt to insert invalid results
            var resultLastnameJSON = controller.Get("name");
            var resultLastname = JsonConvert.DeserializeObject<List<Record>>(resultLastnameJSON);

            //Assert
            Assert.IsNotNull(resultLastname);
            Assert.AreNotEqual(0, resultLastname.Count());
            Assert.AreEqual(6, resultLastname.Count());
            Assert.AreEqual("Zebedane", resultLastname.First().LastName);
            Assert.AreEqual("Cooper", resultLastname.Last().LastName);
        }

        [TestMethod]
        public void GenderTest()
        {
            // Arrange
            RecordsController controller = new RecordsController();

            // Act
            PostValid(ref controller); // insert valid results (6 records)
            PostInvalid(ref controller); // attempt to insert invalid results
            var resultJSON = controller.Get("gender");
            var resultGender = JsonConvert.DeserializeObject<List<Record>>(resultJSON);

            //Assert
            Assert.IsNotNull(resultGender);
            Assert.AreNotEqual(0, resultGender.Count());
            Assert.AreEqual(6, resultGender.Count());
            Assert.IsTrue(resultGender.First().Gender[0] == 'f' || resultGender.First().Gender[0] == 'F');
            Assert.IsTrue(resultGender.Last().Gender[0] == 'm' || resultGender.Last().Gender[0] == 'M');
        }

        [TestMethod]
        public void InvalidInputUnitTest()
        {
            // Arrange
            RecordsController controller = new RecordsController();

            // Act
            PostInvalid(ref controller); // attempt to insert invalid results
            var resultBirthdateJSON = controller.Get("birthdate");
            var resultBirthdate = JsonConvert.DeserializeObject<List<Record>>(resultBirthdateJSON);

            //Assert
            Assert.IsNotNull(resultBirthdate);
            Assert.AreEqual(0, resultBirthdate.Count());
        }

        public void PostValid(ref RecordsController controller)
        {
            controller.Post("Smith Robert M Purple 2/4/1988");
            controller.Post("Zebedane Zebediah Male Purple 3/3/1987");
            controller.Post("Does, Jane, FEM, yellow, 2/19/1942");
            controller.Post("Cooper, Sue, F, y, 5/17/1976");
            controller.Post("McCoy Mark male grn 1/2/1990");
            controller.Post("Danielson Jennifer Female BLUE 12/1/2001");
        }

        public void PostInvalid(ref RecordsController controller)
        {
            controller.Post("blahblahblah");
            controller.Post("Error: incorrect number of elements");
            controller.Post("");
            controller.Post("Person|Mister|Female|yellow|NotADate");
            controller.Post("Rogers|Fred|NotAGender|Love|3/20/1928");
        }

        private List<Record> GetValidTestRecords()
        {
            return new List<Record>
            {
                new Record(new string[] {"Smith", "Robert", "M", "Purple", "11/26/1992" }),
                new Record(new string[] {"Zebedane", "Zebediah", "Male", "Purple", "10/26/1982" }),
                new Record(new string[] {"Does", "Jane", "FEM", "yello", "2/19/1942" }),
                new Record(new string[] {"Cooper", "Sue", "F", "yello", "2/19/1942" }),
                new Record(new string[] {"McCoy", "Mark", "male", "yello", "2/19/1942" }),
                new Record(new string[] {"Danielson", "Jennifer", "Female", "yello", "2/19/1942" })
            };
        }

        private List<string> GetValidTestStrings()
        {
            return new List<string>
            {
                "Smith Robert M Purple 2/4/1988",
                "Zebedane Zebediah Male Purple 3/3/1987",
                "Does, Jane, FEM, yellow, 2/19/1942",
                "Cooper, Sue, F, y, 5/17/1976",
                "McCoy Mark male grn 1/2/1990",
                "Danielson Jennifer Female BLUE 12/1/2001"
            };
        }

        private List<string> GetInvalidTestStrings()
        {
            return new List<string>
            {
                "McCoy|Mark|male|grn|1/2/1990",
                "Danielson | Jennifer | Female | BLUE | 12/1/2001",
                "Zebedane, Zebediah, Male, Purple, 3/3/1987",
                "Cooper, Sue, F, y, 5/17/1976",
                "Smith Robert M Purple 2/4/1988",
                "Does Jane FEM yellow 2/19/1942"
            };
        }
    }
}
