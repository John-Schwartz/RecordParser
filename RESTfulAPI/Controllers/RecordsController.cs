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

        public ParseHelper Helper = new ParseHelper();
        public static List<Record> TestData;

        public RecordsController()
        {
            TestData = new List<Record>();
            //TestData.Add(new Record("Downy", "Robert", "M", "Purple", "11/26/1992"));

        }

        public string Get() => JsonConvert.SerializeObject(TestData);


        [System.Web.Http.Route("Api/Records/{searchWord}")]
        public string Get(string searchWord)
        {
            var helper = new ParseHelper();
            switch (searchWord.ToLowerInvariant())
            {
                case "gender":
                    return JsonConvert.SerializeObject(helper.GetByGender(TestData));

                case "birthdate":
                    return JsonConvert.SerializeObject(helper.GetByBirthdate(TestData));

                case "name":
                    return JsonConvert.SerializeObject(helper.GetByLastname(TestData));

                default: return JsonConvert.SerializeObject(TestData);
            }

        }

        // Request body media type text, no explicit quotation marks e.g.: Downy | Robert | M | Purple | 11/26/1992
        [HttpPost]
        public HttpResponseMessage Post([FromBody] string inputString)
        {
            var response = new HttpResponseMessage();
            try
            {
                //var recordString = await Request.Content.ReadAsStringAsync();
                var recordString = inputString;
                if (string.IsNullOrEmpty(recordString)) return RequestNotAcceptable("Invalid record string. String is null or empty.");

                var helper = new ParseHelper();
                var result = helper.SplitAndSafeStringLine(recordString);
                if (!helper.StringArrayIsValid(result)) return RequestNotAcceptable("Invalid record string. One or more data fields are missing or empty.");

                var newRecord = new Record(result);

                TestData.Add(newRecord);
                response.StatusCode = HttpStatusCode.Accepted;
                response.Content = new StringContent(JsonConvert.SerializeObject(TestData));
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
    }
}
