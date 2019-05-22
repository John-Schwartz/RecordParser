//using RESTfulAPI.Models;
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

        public List<Record> TestData { get; set; }

        public RecordsController()
        {
            TestData = new List<Record>();
            for (var i = 0; i < 5; i++)
            {
                TestData.Add(new Record($"LName{i}", $"FName{i}", $"{(i % 2 == 0 ? 'M' : 'F')}", $"Color{i}", new DateTime(2001 + i, 1 + i, 1 + i)));
            }
        }
        
        public string Get()
        {
            try
            {
                return JsonConvert.SerializeObject(TestData);
            }
            catch (Exception e)
            {
                return $"Exception occurred while returning json data\n" +
                    $"{e.Message}\n" +
                    $"{e.InnerException}";
            }
        }


        [System.Web.Http.Route("Api/Records/{searchWord}")]
        public string Get(string searchWord)
        {
            try
            {
                if (string.IsNullOrEmpty(searchWord)) return "Sort criteria empty or null";
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

                    //case "birthdate_reverse":
                    //    var dobSortedReverse = (from r in TestData
                    //                            orderby r.DateOfBirth descending
                    //                            select r).ToList();
                    //    return JsonConvert.SerializeObject(dobSortedReverse);

                    case "name":
                        var nameSorted = (from r in TestData
                                          orderby r.LastName, r.FirstName
                                          select r).ToList();
                        return JsonConvert.SerializeObject(nameSorted);

                    default: return JsonConvert.SerializeObject(TestData);
                }
            }
            catch (Exception e)
            {
                return $"Exception occurred while returning sorted data\n" +
                    $"Search Term: {searchWord}\n" +
                    $"{e.Message}\n" +
                    $"{e.InnerException}";
            }
        }

        // Request body media type text, no explicit quotation marks e.g.: Downy | Robert | M | Purple | 11/26/1992
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            var response = new HttpResponseMessage();
            try
            {
                var recordString = await Request.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(recordString)) return RequestNotAcceptable("Invalid record string. String is null or empty.");
                
                var helper = new ParseHelper();
                var result = helper.SplitAndSafeStringLine(recordString);
                if (!ValidateStringArray(result)) return RequestNotAcceptable("Invalid record string. One or more data fields are missing or empty.");

                var newRecord = new Record(result);
                TestData.Add(newRecord);

                return Request.CreateResponse(HttpStatusCode.Accepted, JsonConvert.SerializeObject(TestData));
            }
            catch (Exception e)
            {
                response = RequestNotAcceptable($"Post request threw an exception:\n{e.Message}\n{e.InnerException}");
            }

            return response;
        }

        private HttpResponseMessage RequestNotAcceptable(string message)
        {
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.NotAcceptable;
            response.Content = new StringContent(message);

            return response;
        }

        private bool ValidateStringArray(IEnumerable<string> stringArray)
        {
            if (stringArray == null
                || !stringArray.Any()
                || stringArray.Count() < 5
                || !stringArray.ToList().TrueForAll(str => !string.IsNullOrEmpty(str)))
                return false;

            return true;
        }
    }
}
