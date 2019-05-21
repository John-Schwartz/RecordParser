using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESTfulAPI.Controllers;
using RESTfulAPI.Models;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Get_ReturnsAllRecords()
        {
            var expectedResults = GetTestRecords();
            var controller = new RESTfulAPI.Controllers.RecordsController();

            //var expectedResult 
        }

        private List<Record> GetTestRecords()
        {

        }
    }
}
