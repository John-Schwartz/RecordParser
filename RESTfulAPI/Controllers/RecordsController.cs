using RESTfulAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Mvc;
using Newtonsoft.Json;
using RecordParser;
using System.Threading.Tasks;
//using System.Web.Mvc;

namespace RESTfulAPI.Controllers
{

    public class RecordsController : ApiController
    {

        List<Record> TestData { get; set; }

        public RecordsController()
        {
            TestData = new List<Record>();
            for (var i = 0; i < 5; i++)
            {                
                TestData.Add(new Record($"LName{i}", $"FName{i}", $"{(i % 2 == 0 ? 'M' : 'F')}", $"Color{i}", new DateTime(2001 + i, 1 + i, 1 + i)));
            }
            //TestData.Add();
        }

        // GET api/values
        public string Get()
        {
            return JsonConvert.SerializeObject(TestData);
        }
        
        
        [System.Web.Http.Route("Api/Records/{searchWord}")]
        public string Get(string searchWord)
        {
            switch (searchWord.ToLowerInvariant())
            {
                case "gender":
                    var genderSorted = (from r in TestData
                                        orderby r.Gender
                                        select r).ToList();
                    return JsonConvert.SerializeObject(genderSorted);

                case "birthdate":
                    var dobSorted = (from r in TestData
                                     orderby r.DateOfBirth
                                     select r).ToList();
                    return JsonConvert.SerializeObject(dobSorted);

                case "birthdate_reverse":
                    var dobSortedReverse = (from r in TestData
                                     orderby r.DateOfBirth descending
                                     select r).ToList();
                    return JsonConvert.SerializeObject(dobSortedReverse);

                case "name":
                    var nameSorted = (from r in TestData
                                      orderby r.LastName, r.FirstName
                                      select r).ToList();
                    return JsonConvert.SerializeObject(nameSorted);

                default: return JsonConvert.SerializeObject(TestData);
            }
        }

        // Request body media type text, no explicit quotation marks e.g.: Downy | Robert | M | Purple | 11/26/1992
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            var recordString = await Request.Content.ReadAsStringAsync();
            var helper = new RecordParser.ParseHelper();
            var delimArray = new char[] { '|', ',', ' ' };
            var result = helper.SplitAndSafeStringLine(recordString);
            var newRecord = new Record(result);
            TestData.Add(newRecord);
            return new HttpResponseMessage(HttpStatusCode.Accepted);            
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
