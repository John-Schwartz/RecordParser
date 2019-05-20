using RESTfulAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace RESTfulAPI.Controllers
{

    public class RecordsController : ApiController
    {
        public List<Record> TestData { get; set; }

        RecordsController()
        {
            //TestData.Add();
        }

        // GET api/values
        public string Get()
        {
            return JsonConvert.SerializeObject(TestData);
        }

        // GET api/values/5
        public string Get(string searchWord)
        {
            switch (searchWord)
            {
                case "gender":
                    var genderSorted = "";
                    break;

                case "birthdate":
                    var dobSorted = "";
                    break;

                case "name":
                    var nameSorted = "";
                    break;
            }
            return "value";
        }

        // POST api/values
        //public HttpResponseMessage Post([FromBody]Record record)
        //{
        //    var recordString = $"{record.LastName}, {record.FirstName}, {record.Gender}, {record.FavoriteColor}, {record.DateOfBirth.ToString("M/d/yyyy")}";

        //    if (ModelState.IsValid)
        //    {
        //        _service.Add(comment);
        //        return Request.CreateResponse(HttpStatusCode.Created, comment);
        //    }
        //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //}
    }
}
