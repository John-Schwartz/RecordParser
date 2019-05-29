using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using ParseHelperLibrary;

namespace RESTfulAPI.Controllers
{

    public class RecordsController : ApiController
    {

        private ParseHelper Helper = new ParseHelper();
        private static List<Record> TestData;

        public RecordsController() => TestData = new List<Record>();

        // Default get returns full unsorted JSON collection of test data
        public string Get() => JsonConvert.SerializeObject(TestData);


        // Returns JSON of full test list, sorted by the search word, 
        // or unsorted if passed an unrecognized search value
        [System.Web.Http.Route("Api/Records/{searchWord}")]
        public string Get(string searchWord)
        {
            switch (searchWord.ToLowerInvariant())
            {
                // GET: ../records/gender
                case "gender":
                    return JsonConvert.SerializeObject(Helper.GetByGender(TestData));

                // GET: ../records/birthdate
                case "birthdate":
                    return JsonConvert.SerializeObject(Helper.GetByBirthdate(TestData));

                // GET: ../records/name
                case "name":
                    return JsonConvert.SerializeObject(Helper.GetByLastname(TestData));

                default: return JsonConvert.SerializeObject(TestData);
            }

        }

        // Request body media type text e.g.: Downy | Robert | M | Purple | 11/26/1992
        // POST: ../records/
        [HttpPost]
        public HttpResponseMessage Post([FromBody] string inputString)
        {
            var response = new HttpResponseMessage();
            try
            {
                // If the string is empty, return NotAcceptable
                if (string.IsNullOrWhiteSpace(inputString)) return RequestNotAcceptable("Invalid record string. String is null or empty.");

                // Split the input string by the expected delimiters and clean up output
                IEnumerable<string> result = Helper.SplitAndSafeStringLine(inputString);

                // if the Split/trimmed string array is not valid, return NotAcceptable
                if (!Helper.StringCollectionIsValid(result)) return RequestNotAcceptable("Invalid record string. One or more data fields are missing or empty.");

                var newRecord = Record.CreateRecord(result);

                // if the string array is valid, add it to test data and return the full json
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
