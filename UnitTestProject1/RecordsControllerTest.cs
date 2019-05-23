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

        public void GenderSortTest()
        {
            var testRecords = GetTestRecords();
            var controller = new RecordsController();

            var genderSorted = from r in testRecords
                               orderby r.Gender
                               select r;
            var expectedResults = JsonConvert.SerializeObject(genderSorted);
            var controllerResult = controller.Get("gender");
            Assert.AreEqual(controllerResult, expectedResults);
            //Assert.IsTrue(.First(rec => rec));
        }

        [TestMethod]
        public void GetWithSearch_ReturnsAllSortedRecords()
        {
            var testRecords = GetTestRecords();
            var controller = new RecordsController();

            var dobSorted = from r in testRecords
                            orderby r.DateOfBirth
                            select r;
            var expectedResults = JsonConvert.SerializeObject(dobSorted);
            var controllerResult = controller.Get("birthdate");
            Assert.AreEqual(controllerResult, expectedResults);

            var nameSorted = (from r in testRecords
                              orderby r.LastName, r.FirstName
                              select r).ToList();
            expectedResults = JsonConvert.SerializeObject(nameSorted);
            controllerResult = controller.Get("name");
            Assert.AreEqual(controllerResult, expectedResults);

            // Default json returned with anything other than expected strings
            var controllerResultDefault = controller.Get("11blahblahokaskasasgasdgasdgalfeiwfqnk");
            Assert.AreEqual(controllerResultDefault, expectedResults);
        }

        [TestMethod]
        public void Post_AddsRecordAndReturnsFullRecordJson()
        {
            // Arrange
            RecordsController controller = new RecordsController();
            Record newRecord = new Record("Downy", "Robert", "M", "Purple", "11/26/1992");
            var testRecords = GetTestRecords();

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
            testRecords.Add(newRecord);

            var response = controller.Post();
            response.Result.TryGetContentValue(out HttpResponseMessage contentVal);

            //unit testing breaks with async functions
            //var returnedContent = await contentVal.Content.ReadAsStringAsync();


            // Assert
            Assert.AreEqual(controller.Get(), JsonConvert.SerializeObject(testRecords));
            //Assert.AreEqual(JsonConvert.SerializeObject(await returnedContent), JsonConvert.SerializeObject(testRecords));
        }

        private List<Record> GetValidTestRecords()
        {
            return new List<Record>
            {
                new Record("Smith", "Robert", "M", "Purple", "11/26/1992"),
                new Record("Zebedane", "Zebediah", "Male", "Purple", "10/26/1982"),
                new Record("Does", "Jane", "FEM", "yello", "2/19/1942" ),
                new Record("Cooper", "Sue", "F", "yello", "2/19/1942" ),
                new Record("McCoy", "Mark", "male", "yello", "2/19/1942" ),
                new Record("Danielson", "Jennifer", "Female", "yello", "2/19/1942" )
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
                "Smith M Purple 2/4/1988",
                "Zebedane Zebediah Male Purple Unknown",
                "Does, Jane, green, FEM, 5/17/1976",
                "Cooper, Sue, F, y, 99/99/9999",
                "McCoy Mark male grn 1 / 2 / 1990",
                "Danielson Jennifer true BLUE 122/1/2001"
            };
        }



        //private List<Record> GetBadTestRecords()
        //{
        //    var badStringArray = new string[] { null, string.Empty, "false", "aaaaaaaa", "2001-11-26" };
        //    return new List<Record>
        //    {
        //        new Record(null, string.Empty, "false", "aaaaaaaa", new DateTime()),
        //        new Record(badStringArray)
        //    };
        //}


    }
}
