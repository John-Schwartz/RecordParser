using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESTfulAPI.Controllers;
using RESTfulAPI.Models;
using Newtonsoft.Json;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

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

        [TestMethod]
        public void GetWithSearch_ReturnsAllSortedRecords()
        {
            var testRecords = GetTestRecords();
            var controller = new RecordsController();

            var genderSorted = from r in testRecords
                               orderby r.Gender
                               select r;
            var expectedResults = JsonConvert.SerializeObject(genderSorted);
            var controllerResult = controller.Get("gender");
            Assert.AreEqual(controllerResult, expectedResults);

            var dobSorted = from r in testRecords
                            orderby r.DateOfBirth
                            select r;
            expectedResults = JsonConvert.SerializeObject(dobSorted);
            controllerResult = controller.Get("birthdate");
            Assert.AreEqual(controllerResult, expectedResults);

            var nameSorted = (from r in testRecords
                              orderby r.LastName, r.FirstName
                              select r).ToList();
            expectedResults = JsonConvert.SerializeObject(nameSorted);
            controllerResult = controller.Get("name");
            Assert.AreEqual(controllerResult, expectedResults);
        }

        [TestMethod]
        public void Post_AddsRecordAndReturnsFullRecordJson()
        {
            // Arrange
            RecordsController controller = new RecordsController();
                       
            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/records"),
                Method = HttpMethod.Post,
                Content = new StringContent("Downy | Robert | M | Purple | 11/26/1992")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "records" } });

            // Act
            Record newRecord = new Record("Downy", "Robert", "M", "Purple", new DateTime(1992, 11, 26));
            var testRecords = GetTestRecords();
            testRecords.Add(newRecord);

            var response = controller.Post();
            
            // Assert
            Assert.AreEqual(controller.Get(), JsonConvert.SerializeObject(testRecords));
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
