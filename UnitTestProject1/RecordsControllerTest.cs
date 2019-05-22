using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESTfulAPI.Controllers;
using RESTfulAPI.Models;
using Newtonsoft.Json;
using System.Web;

namespace UnitTestProject1
{
    [TestClass]
    public class RecordsControllerTest
    {
        [TestMethod]
        public void Get_ReturnsAllRecords()
        {
            var expectedResults = JsonConvert.SerializeObject(GetTestRecords());
            var controller = new RecordsController();

            var controllerResult = controller.Get();
            Assert.AreEqual(controllerResult, expectedResults);
        }

        public void GetWithSearch_ReturnsAllSortedRecords()
        {
            var testRecords = GetTestRecords();
            //var genderSorted = (from r in testRecords
            //                   orderby r.Gender
            //                   select r).ToList();
            var expectedResults = JsonConvert.SerializeObject(GetTestRecords());
            var controller = new RecordsController();

            var controllerResult = controller.Get("gender");
            Assert.AreEqual(controllerResult, expectedResults);
        }

        private List<Record> GetTestRecords()
        {
            var testRecords = new List<Record>();
            for (var i = 0; i < 5; i++)
            {
                testRecords.Add(new Record($"LName{i}", $"FName{i}", $"{(i % 2 == 0 ? 'M' : 'F')}", $"Color{i}", new DateTime(2001 + i, 1 + i, 1 + i)));
            }
            return testRecords;
        }
    }
}
