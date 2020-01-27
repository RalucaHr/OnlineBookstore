using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DTA.Web.Controllers
{
    public class WebServController : ApiController
    {
        // GET: api/WebServ
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/WebServ/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/WebServ
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/WebServ/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/WebServ/5
        public void Delete(int id)
        {
        }
    }
}
