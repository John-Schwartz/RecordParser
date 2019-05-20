using RESTfulAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RESTfulAPI.Controllers
{

    public class RecordsController : ApiController
    {

        // GET api/values
        public IEnumerable<Record> Get()
        {
            return new List<Record> { };
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
        public void Post([FromBody]Record record)
        {
            var recordString = $"{record.LastName}, {record.FirstName}, {record.Gender}, {record.FavoriteColor}, {record.DateOfBirth.ToString("M/d/yyyy")}";
        }
    }
}
