using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication.Controllers
{
    public class UserController : ApiController
    {

        [Authorize]
        // GET: api/User/5
        public string Get()
        {
            return "value";
        }

        [Authorize]
        public void Post([FromBody]string value)
        {

        }


        [Authorize]
        public void Delete(int id)
        {
        }
    }
}
