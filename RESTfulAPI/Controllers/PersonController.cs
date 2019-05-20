using RESTfulAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RESTfulAPI.Controllers
{
    //public class RestAPIViewModel
    //{
    //}

    public class PersonController : ApiController
    {
        
        
        
        
        
        // GET api/values
        public IEnumerable<Person> Get()
        {
            return new Person[] { };
        }

        // GET api/values/5
        public string Get(string gender)
        {
            return "value";
        }
        public string Get(DateTime dob)
        {
            return "value";
        }
        public string Get(string lastName, string firstName)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]Person person)
        {

        }



        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }


}
